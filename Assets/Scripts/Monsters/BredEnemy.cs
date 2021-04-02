using PlayerClasses;
using System.Collections.Generic;
using UnityEngine;

public sealed class BredEnemy : Enemy
{
    private readonly List<Transform> eyes = new List<Transform>();
    [SerializeField] private Transform head;
    private void Awake()
    {                      
        for (int i = 0; i < head.childCount; i++)
        {
            eyes.Add(head.GetChild(i));
        }

        base.Init(2, 3, 25, 500);        
    }

    private void Update()
    {
        HarassmentEnemy();
        if (currentTPAS >= 0 || currentEnemy != null)
        {
            SetEnemy(null);
        }
    }
    private void FixedUpdate()
    {
        RayCastToEnemy();
    }
    protected override void Death(float health)
    {
        if (health > UniqueVariables.MinHealth)
            return;
        currentTPAS = 0;
        mAgent.enabled = false;
        SetAnimationClip(AnimationsContainer.Death);
        currentState = states.isDied;
        GetComponent<BoxCollider>().enabled = false;
        enabled = false;
    }

    protected override void HarassmentEnemy()
    {
        if (currentState == states.isDied)
            return;
        SetAnimationClip(AnimationsContainer.MoveToPerson);
        if (currentEnemy == null)
        {
            SetTarget(defenderPoint);
            return;
        }

        SetTarget(currentEnemy.transform);

        if (mAgent.remainingDistance <= mAgent.stoppingDistance)
        {
            SetAnimationClip(AnimationsContainer.Attack);
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
        mAnim.SetBool(AnimationsContainer.MoveToPerson, false);
        mAnim.SetBool(AnimationsContainer.Death, false);
        mAnim.SetBool(AnimationsContainer.Attack, false);


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
                currentTarget = other.transform;
                RayCastToEnemy();
            }
        }
    }

    protected override void LookOnTarget()
    {
        Vector3 startRot = transform.localEulerAngles;
        transform.LookAt(currentTarget.transform);
        transform.localEulerAngles = new Vector3(startRot.x, transform.localEulerAngles.y, 0);
    }
    protected override void SetTarget(Transform target)
    {
        currentTarget = target;
        base.SetTarget(target);
        if (mAgent.remainingDistance <= mAgent.stoppingDistance * 2)
        {
            SetAnimationClip(AnimationsContainer.MoveToPerson, false);
        }
    }

    protected override float timePursuitAfterSaw()
    {
        return 5f;
    }
    private void RayCastToEnemy()
    {
        if (currentEnemyForewer)
            return;

        if (currentTarget == null)
            return;
        foreach (var e in eyes)
        {
            if (Physics.Linecast(e.position, currentTarget.position, out RaycastHit hit))
            {
                if (hit.transform.TryGetComponent<BasicNeeds>(out var bn))
                {
                    if (Vector3.Distance(e.position, currentTarget.position) > UVariables.SeeDistance)
                        break;
                    if (currentEnemy != bn)
                    {
                        SetEnemy(bn);
                        return;
                    }
                }
            }
        }
        SetEnemy(null);
    }

    protected override string Type()
    {
        return TypesEnemies.Bred;
    }
    /* void OnDrawGizmos()
{
    try
    {
        foreach (var e in eyes)
        {
            bool isHit = Physics.Linecast(e.position, currentTarget.position);
            if (isHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(e.position, currentTarget.position);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(e.position, currentTarget.position);
            }
        }
    }
    catch
    {
    }
}*/
}
