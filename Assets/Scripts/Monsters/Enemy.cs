using PlayerClasses;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// наследники класса - враги, их можно убивать. 
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    protected abstract float timePursuitAfterSaw();//время преследования после обнаружения игрока
    protected float currentTPAS;//текущее время преследования
    [SerializeField] protected Transform defenderPoint;//защитная точка бреда
    protected Transform currentTarget;// текущая цель
    protected NavMeshAgent mAgent;
    protected Animator mAnim;
    protected float distanceForAttack = 2;// дистанция для атаки
    protected float powerInjure = 3;// сила удара
    protected float seeDistance = 25;
    protected enum states { wait, attack, isDied };
    protected states currentState;
    private float health;
    public float Health
    {
        get { return health; }
        protected set
        {
            if (value <= MinHealth)
            {
                Death();
                value = MinHealth;
            }
            health = value;
            ChangeHealthEvent?.Invoke(value);
        }
    }

    public const float MinHealth = 0;
    protected BasicNeeds currentEnemy;// текущий противник
    protected bool currentEnemyForewer;// при включенной булевой враг монстра никогда не сменит цель

    public delegate void HealthHandler(float health);
    public event HealthHandler ChangeHealthEvent;// событие смены здоровья

    protected class AnimationsContainer
    {
        public const string MoveToPerson = "MoveToPerson";
        public const string Death = "Death";
        public const string Attack = "Attack";
    }


    /// <summary>
    /// функция нанесения урона монстром
    /// </summary>
    protected virtual void Attack()
    {
        currentEnemy.InjurePerson(powerInjure * Time.deltaTime);
    }
    /// <summary>
    /// функция установки цели задания состояний
    /// </summary>
    protected abstract void HarassmentEnemy();
    /// <summary>
    /// функция получения урона монстром
    /// </summary>
    /// <param name="value"></param>
    public virtual void InjureEnemy(float value,BasicNeeds bn)
    {
        if (bn != null)
            SetEnemy(bn);
        Health -= value;
    }
    /// <summary>
    /// функция смерти
    /// </summary>
    protected abstract void Death();
    /// <summary>
    /// задаёт несменяемость текущей цели
    /// </summary>
    /// <param name="value"></param>
    public void SetCurrentEnemyForewer(bool value)
    {
        currentEnemyForewer = value;
    }
    /// <summary>
    /// функция установки противника
    /// </summary>
    /// <param name="enemy"></param>
    public void SetEnemy(BasicNeeds enemy)
    {
        if (currentTPAS >= 0 && enemy == null)
        {
            currentTPAS -= Time.deltaTime;
            return;
        }
        currentEnemy = enemy;
        currentTPAS = timePursuitAfterSaw();
    }
    /// <summary>
    /// функция установки цели
    /// </summary>
    /// <param name="target"></param>
    protected virtual void SetTarget(Transform target)
    {    
            mAgent.SetDestination(target.position);
    }
    /// <summary>
    /// функция поворота к цели
    /// </summary>
    protected abstract void LookOnTarget();
    /// <summary>
    /// функция задачи анимаций
    /// </summary>
    /// <param name="state"></param>
    /// <param name="value"></param>
    protected abstract void SetAnimationClip(string state = "", bool value = true);
}
