using PlayerClasses;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected NavMeshAgent mAgent;
    protected Animator mAnim;
    protected float distanceForAttack = 2;
    protected float powerInjure = 3;
    protected enum states { wait, attack, isDied };
    protected states currentState;
    private float health;
    public float Health
    {
        get { return health; }
        protected set
        {
            if (value <= MinHealth)
                Death();
            health = value;
            ChangeHealthEvent?.Invoke(value);
        }
    }

    public const float MinHealth = 0;
    protected BasicNeeds currentEnemy;
    public BasicNeeds CurrentEnemy
    {
        get => currentEnemy;
        protected set
        {
            currentEnemy = value;
        }
    }
    protected bool currentEnemyForewer;

    public delegate void HealthHandler(float health);
    public event HealthHandler ChangeHealthEvent;

    protected abstract void Attack();
    protected abstract void HarassmentEnemy();
    public abstract void InjureEnemy(float value);
    protected abstract void Death();

    public void SetCurrentEnemyForewer(bool value)
    {
        currentEnemyForewer = value;
    }
    public void SetEnemy(BasicNeeds enemy)
    {
        currentEnemy = enemy;
    }
    protected abstract void SetAnimationClip(string state = "");
}
