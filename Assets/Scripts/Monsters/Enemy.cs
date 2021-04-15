using PlayerClasses;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// наследники класса - враги, их можно убивать. 
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    public UniqueVariables UVariables { get; private set; }
    public class UniqueVariables
    {
        public delegate void HealthHandler(float health);
        public event HealthHandler ChangeHealthEvent;// событие смены здоровья
        public float DistanceForAttack { get; private set; } = 0;// дистанция для атаки
        public float PowerInjure { get; private set; } = 0;// сила удара
        public float SeeDistance { get; private set; } = 0;//радиус взора

        public const float MinHealth = 0;

        private float health;
        public float Health
        {
            get { return health; }
            set
            {
                if (value <= MinHealth)
                {
                    value = MinHealth;
                }
                health = value;
                ChangeHealthEvent?.Invoke(value);
            }
        }
        public UniqueVariables(float SdistanceForAttack, float SpowerInjure, float SseeDistance, float Shealth)
        {
            this.DistanceForAttack = SdistanceForAttack;
            this.PowerInjure = SpowerInjure;
            this.SeeDistance = SseeDistance;
            this.Health = Shealth;
        }
    }

    public class TypesEnemies
    {
        public const string Bred = "Bred";
        public const string BloodDog = "BloodDog";
    }
    protected abstract string Type();
    protected abstract float timePursuitAfterSaw();//время преследования после обнаружения игрока
    protected float currentTPAS;//текущее время преследования
    [SerializeField] protected Transform defenderPoint;//защитная точка бреда
    protected Transform currentTarget;// текущая цель
    protected NavMeshAgent mAgent;
    protected Animator mAnim;
    protected enum states { wait, attack, isDied };
    protected states currentState;


    protected BasicNeeds currentEnemy;// текущий противник
    protected bool currentEnemyForewer;// при включенной булевой враг монстра никогда не сменит цель


    protected void Init(float distanceForAttack, float powerInjure, float seeDistance, float health)
    {        
        UVariables = new UniqueVariables(distanceForAttack, powerInjure, seeDistance, health);
        mAgent = GetComponent<NavMeshAgent>();
        mAnim = GetComponent<Animator>();

        mAgent.stoppingDistance = UVariables.DistanceForAttack;
        UVariables.ChangeHealthEvent += Death;
    }
    protected class AnimationsContainer
    {
        public const string MoveToPerson = "MoveToPerson";
        public const string Death = "Death";
        public const string Attack = "Attack";
    }


    /// <summary>
    /// функция нанесения урона монстром
    /// </summary>
    protected void Attack()
    {
        currentEnemy.InjurePerson(UVariables.PowerInjure * Time.deltaTime);
    }
    /// <summary>
    /// функция установки цели задания состояний
    /// </summary>
    protected abstract void HarassmentEnemy();
    /// <summary>
    /// функция получения урона монстром
    /// </summary>
    /// <param name="value"></param>
    public virtual void InjureEnemy(float value, BasicNeeds bn)
    {
        if (bn != null)
            SetEnemy(bn);
        UVariables.Health -= value;
    }
    /// <summary>
    /// функция смерти
    /// </summary>
    protected abstract void Death(float health);
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
    protected void OnDestroy()
    {
        UVariables.ChangeHealthEvent -= Death;
    }
}
