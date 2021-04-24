using PlayerClasses;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// наследники класса - враги, их можно убивать. 
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private Transform centerEnemy;
    [SerializeField] protected float Fov = 90;
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
    [SerializeField] protected Transform defenderPoint;//защитная точка бреда
    protected Transform currentTarget;// текущая цель
    protected Vector3 lastTargetPos;
    protected NavMeshAgent mAgent;
    protected Animator mAnim;
    [SerializeField] protected LayerMask layerMasks;
    [SerializeField] private List<Transform> eyes = new List<Transform>();

    protected BasicNeeds currentEnemy;// текущий противник
    protected bool currentEnemyForewer;// при включенной булевой враг монстра никогда не сменит цель


    protected void Init(float distanceForAttack, float powerInjure, float seeDistance, float health)
    {
        UVariables = new UniqueVariables(distanceForAttack, powerInjure, seeDistance, health);
        mAgent = GetComponent<NavMeshAgent>();
        mAnim = GetComponent<Animator>();

        mAgent.stoppingDistance = UVariables.DistanceForAttack;
        UVariables.ChangeHealthEvent += Death;

        if (!defenderPoint)
        {
            var dp = new GameObject($"DefenderPointFor{name}").transform;
            dp.position = transform.position;
            defenderPoint = dp;
        }

        currentTarget = defenderPoint;
        lastTargetPos = currentTarget.position;
    }
    protected class AnimationsContainer
    {
        public const string MoveToPerson = "MoveToPerson";
        public const string Death = "Death";
        public const string Attack = "Attack";
    }

    private void FixedUpdate()
    {
        RayCastToEnemy(BasicNeeds.Instance.transform.position);
    }
    private bool FindEnemies()
    {
        if (currentEnemyForewer)
            return false;

        Vector3 targetDir = BasicNeeds.Instance.transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        float borderMin = Mathf.Sin(-Fov / 2) * Mathf.Rad2Deg;
        float borderMax = Mathf.Sin(Fov / 2) * Mathf.Rad2Deg;

        bool isIntersected = angle > borderMin && angle < borderMax;
        return isIntersected;

    }
    private void RayCastToEnemy(Vector3 end)
    {
        Transform target = defenderPoint;
        BasicNeeds enemy = null;
        foreach (var e in eyes)
        {
            if (Physics.Linecast(e.position, end, out RaycastHit hit, layerMasks, QueryTriggerInteraction.Ignore))
            {
                if (!hit.transform.TryGetComponent<BasicNeeds>(out var bn))
                    continue;
                if (Vector3.Distance(e.position, end) > UVariables.SeeDistance)
                    continue;
                if (!FindEnemies())
                    continue;

                target = hit.transform;
                enemy = bn;
            }
        }
        SetEnemy(enemy);
        SetTarget(target);
    }
    /// <summary>
    /// функция нанесения урона монстром
    /// </summary>
    protected void Attack()
    {
        currentEnemy.InjurePerson(UVariables.PowerInjure * Time.deltaTime);
    }
    /// <summary>
    /// функция получения урона монстром
    /// </summary>
    /// <param name="value"></param>
    public virtual void InjureEnemy(float value)
    {
        SetEnemy(BasicNeeds.Instance);
        lastTargetPos = BasicNeeds.Instance.transform.position;
        UVariables.Health -= value;
    }
    /// <summary>
    /// функция смерти
    /// </summary>
    protected void Death(float health)
    {
        if (health > UniqueVariables.MinHealth)
            return;
        mAgent.enabled = false;
        SetAnimationClip(AnimationsContainer.Death);
        enabled = false;
    }
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
        currentEnemy = enemy;        
    }
    /// <summary>
    /// функция установки цели
    /// </summary>
    /// <param name="target"></param>
    protected virtual void SetTarget(Transform target)
    {
        currentTarget = target;
        if (currentEnemy)//враг не потерян
        {
            lastTargetPos = currentEnemy.transform.position;// запись в последнюю известную точку                         
        }
        else if (Vector3.Distance(centerEnemy.position, lastTargetPos) < mAgent.stoppingDistance)
        {
            lastTargetPos = defenderPoint.position;
        }
        mAgent.SetDestination(lastTargetPos);

        if (mAgent.remainingDistance > mAgent.stoppingDistance)// если до цели не дошёл агент
        {
            SetAnimationClip(AnimationsContainer.MoveToPerson);// идти к цели
        }
        else if (mAgent.remainingDistance <= mAgent.stoppingDistance && currentEnemy)// если дошёл
        {//атаковать
            SetAnimationClip(AnimationsContainer.Attack);
            Attack();
        }
        else
        {
            SetAnimationClip();
        }
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
    protected void SetAnimationClip(string state = "", bool value = true)
    {
        mAnim.SetBool(AnimationsContainer.MoveToPerson, false);
        mAnim.SetBool(AnimationsContainer.Death, false);
        mAnim.SetBool(AnimationsContainer.Attack, false);
        if (state != string.Empty)
            mAnim.SetBool(state, value);
    }

    protected void OnDestroy()
    {
        UVariables.ChangeHealthEvent -= Death;
    }
}
