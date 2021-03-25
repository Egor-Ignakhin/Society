using UnityEngine;
using UnityEngine.AI;

public sealed class DogEnemy : Enemy
{
    private NavMeshAgent mAgent;
    private Animator mAnim;
    private enum states { wait, attack, isDied};
    private states currentState;
    private float distanceForAttack = 2;
    private float powerInjure = 3;
    [SerializeField] private Transform head;
    private void Awake()
    {
        mAgent = GetComponent<NavMeshAgent>();
        mAnim = GetComponent<Animator>();
        Health = 100;
    }
    protected override void Attack()
    {
        currentEnemy.InjurePerson(powerInjure * Time.deltaTime);
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
        Vector3 startRot = transform.localEulerAngles;
        mAgent.transform.LookAt(currentEnemy.transform);
        transform.localEulerAngles = new Vector3(startRot.x, transform.localEulerAngles.y, 0);
        head.transform.LookAt(currentEnemy.transform);
        SetAnimationClip();
    }
    private void SetAnimationClip()
    {
        float dist = Vector3.Distance(transform.position, currentEnemy.transform.position);
        if (dist < distanceForAttack)
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

    public override void InjureEnemy(float value)
    {
        Debug.Log("hit");
        Health -= value;
    }

    protected override void Death()
    {
        Debug.Log("death");
        currentState = states.isDied;
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
}
