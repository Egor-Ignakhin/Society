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
        if(currentTPAS >= 0)
        {
            SetEnemy(null);
        }
        else if(currentEnemy != null)
        {
            SetEnemy(null);
        }
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
        SetAnimationClip("IsMoveToPerson");
        if (currentEnemy == null)
        {
            SetTarget(defenderPoint);
            return;
        }

        SetTarget(currentEnemy.transform);

        if (mAgent.remainingDistance <= mAgent.stoppingDistance * 2)
        {
            SetAnimationClip("Attack");
            currentState = states.attack;
            Attack();
        }
        else
        {
            currentState = states.wait;
        }
    }
    protected override void SetAnimationClip(string state, bool value = true)
    {
        mAnim.SetBool("IsMoveToPerson", false);
        mAnim.SetBool("Death", false);
        mAnim.SetBool("Attack", false);


        mAnim.SetBool(state, value);

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
        if (other.GetComponent<BasicNeeds>())
            SetEnemy(null);
    }

    protected override void LookOnTarget()
    {
        Vector3 startRot = transform.localEulerAngles;
        transform.LookAt(currentPoint.transform);
        transform.localEulerAngles = new Vector3(startRot.x, transform.localEulerAngles.y, 0);
    }
    protected override void SetTarget(Transform target)
    {
        currentPoint = target;
        base.SetTarget(target);
        LookOnTarget();
        if (mAgent.remainingDistance <= mAgent.stoppingDistance * 2)
        {
            SetAnimationClip("IsMoveToPerson", false);
        }
    }

    protected override float timePursuitAfterSaw()
    {
        return 5f;
    }
}
