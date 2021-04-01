using PlayerClasses;
using UnityEngine;
using UnityEngine.AI;

public class BredEnemy : Enemy
{
    private void Awake()
    {
        mAgent = GetComponent<NavMeshAgent>();
        mAnim = GetComponent<Animator>();
        Health = 500;
    }
    public override void InjureEnemy(float value)
    {
        Health -= value;
    }

    protected override void Attack()
    {
        currentEnemy.InjurePerson(powerInjure * Time.deltaTime);
    }
    private void Update()
    {
        HarassmentEnemy();
        Debug.Log(mAnim.GetBool("Attack"));

    }
    protected override void Death()
    {
        SetAnimationClip("Death");
        currentState = states.isDied;
        GetComponent<BoxCollider>().enabled = false;
    }

    protected override void HarassmentEnemy()
    {
        if (currentState == states.isDied)
            return;
        if (!currentEnemy)
            return;
        mAgent.SetDestination(currentEnemy.transform.position);
        Vector3 startRot = transform.localEulerAngles;
        transform.LookAt(currentEnemy.transform);
        transform.localEulerAngles = new Vector3(startRot.x, transform.localEulerAngles.y, 0);


        if (mAgent.remainingDistance <= mAgent.stoppingDistance * 2)
        {           
           SetAnimationClip("Attack");
            currentState = states.attack;
            Attack();
        }
        else
        {
            SetAnimationClip("IsMoveToPerson");
            currentState = states.wait;
        }
    }
    protected override void SetAnimationClip(string state)
    {
        mAnim.SetBool("IsMoveToPerson", false);
        mAnim.SetBool("Death", false);
        mAnim.SetBool("Attack",false);


        mAnim.SetBool(state, true);

    }

    private void OnTriggerStay(Collider other)
    {
        if (currentEnemyForewer)
            return;
        if (other.TryGetComponent<BasicNeeds>(out var bn))
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
}
