using PlayerClasses;
using UnityEngine;
using UnityEngine.AI;

public sealed class DogEnemy : Enemy
{    
    [SerializeField] private Transform head;
    private void Awake()
    {
        mAgent = GetComponent<NavMeshAgent>();
        mAnim = GetComponent<Animator>();
        base.Init(2, 3, 5, 100);
    }

    private void Update()
    {
        HarassmentEnemy();
    }

    protected override void HarassmentEnemy()
    {
        if (currentState == states.isDied)
            return;
        if (!currentEnemy)
            return;
        mAgent.SetDestination(currentEnemy.transform.position);       
        SetAnimationClip();
    }
    protected override void SetAnimationClip(string state = "",bool value = true)
    {
        float dist = Vector3.Distance(transform.position, currentEnemy.transform.position);
        if (dist < mAgent.stoppingDistance)
        {
            mAnim.SetInteger("IsMoveToPerson", 0);
            mAnim.SetInteger("IsAttack", 1);
            currentState = states.attack;
            Attack();         
        }
        else
        {
            mAnim.SetInteger("IsAttack", 0);
            mAnim.SetInteger("IsMoveToPerson", 1);
        }
        currentState = states.wait;
    }

    protected override void Death(float health)
    {
        if (health > UniqueVariables.MinHealth)
            return;
        Debug.Log("death");
        currentState = states.isDied;
        mAgent.enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (currentEnemyForewer)
            return;
        if(other.TryGetComponent<BasicNeeds>(out var bn))
        {
            if (currentEnemy != bn)
            {
                SetEnemy(bn);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (currentEnemyForewer)
            return;
        if (other.TryGetComponent<BasicNeeds>(out var bn))
            SetEnemy(null);
    }

    protected override void LookOnTarget()
    {
        Vector3 startRot = transform.localEulerAngles;
        mAgent.transform.LookAt(currentEnemy.transform);
        transform.localEulerAngles = new Vector3(startRot.x, transform.localEulerAngles.y, 0);
        head.transform.LookAt(currentEnemy.transform);
    }

    protected override float timePursuitAfterSaw()
    {
      return 5f;
    }

    protected override string Type()
    {
        return TypesEnemies.BloodDog;
    }
}
