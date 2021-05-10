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
    [SerializeField] protected float fov = 90;
    [SerializeField] protected float seeDistance;
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
    protected Transform target;// текущая цель
    protected Vector3 lastTargetPos;
    protected Vector3 possibleTargetPos;
    protected NavMeshAgent mAgent;
    protected Animator mAnim;
    [SerializeField] protected LayerMask layerMasks;
    [SerializeField] private List<Transform> eyes = new List<Transform>();

    protected BasicNeeds enemy;// текущий противник

    public delegate void EnemyEvent();
    public event EnemyEvent DeathEvent;

    private float WaitTarget;
    private bool wasInjured;

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

        target = defenderPoint;
        lastTargetPos = target.position;
    }
    protected class AnimationsContainer
    {
        public const string MoveToPerson = "MoveToPerson";
        public const string Death = "Death";
        public const string Attack = "Attack";
    }

    private void Update()
    {
        RayCastToEnemy();
        DebugDraw();
    }
    private bool CalculateDistance(Vector3 pos)
    {
        NavMeshPath path = new NavMeshPath();
        float dist = 0;
        if (mAgent.CalculatePath(pos, path))
        {
            for (int x = 1; x < path.corners.Length; x++)
                dist += Vector3.Distance(path.corners[x - 1], path.corners[x]);
        }
        return dist <= UVariables.SeeDistance;
    }
    private bool CalculateAngle(Vector3 pos)
    {
        Vector3 targetDir = pos - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        float borderMin = Mathf.Sin(fov) * (fov / 90) * Mathf.Rad2Deg;
        float borderMax = -Mathf.Sin(fov) * (fov / 90) * Mathf.Rad2Deg;

        bool isIntersected = (angle > borderMin && angle < borderMax) || wasInjured;
        
        return isIntersected;

    }
    private void RayCastToEnemy()
    {
        Vector3 pos = BasicNeeds.Instance.transform.position;
        foreach (var e in eyes)
        {
            if (Physics.Linecast(e.position, pos, out RaycastHit hit, layerMasks, QueryTriggerInteraction.Ignore))
            {
                if (!CalculateDistance(pos))
                    continue;
                if (!CalculateAngle(pos))
                    continue;

                if (hit.transform.TryGetComponent<BasicNeeds>(out var bn))
                {
                    SetEnemy(bn);
                    return;
                }
            }
        }
        wasInjured = false;
        SetEnemy(null);
    }
    /// <summary>
    /// функция нанесения урона монстром
    /// </summary>
    protected void Attack()
    {
        enemy.InjurePerson(UVariables.PowerInjure * Time.deltaTime);
    }
    /// <summary>
    /// функция получения урона монстром
    /// </summary>
    /// <param name="value"></param>
    public virtual void InjureEnemy(float value)
    {
        wasInjured = true;      
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
        SetAnimationClip();
        mAnim.SetTrigger(AnimationsContainer.Death);
        mAnim.applyRootMotion = true;
        enabled = false;
        DeathEvent.Invoke();
    }

    /// <summary>
    /// функция установки противника
    /// </summary>
    /// <param name="enemy"></param>
    public void SetEnemy(BasicNeeds enemy)
    {
        this.enemy = enemy;
        SetTarget(enemy ? enemy.transform : defenderPoint);
        if (enemy)
            WaitTarget = 5;
    }
    /// <summary>
    /// функция установки цели
    /// </summary>
    /// <param name="target"></param>
    protected virtual void SetTarget(Transform t)
    {
        this.target = t;
        if (enemy)//враг не потерян
        {
            lastTargetPos = enemy.transform.position;// запись в последнюю известную точку                         
        }
        else if (Vector3.Distance(centerEnemy.position, lastTargetPos) < mAgent.stoppingDistance)
        {
            if (WaitTarget < 0)
            {
                lastTargetPos = defenderPoint.position;
            }
            else
            {
                WaitTarget -= Time.deltaTime;
                var direction = (possibleTargetPos - transform.position).normalized;
                direction.y = 0f;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 1);
            }
        }
        else if (WaitTarget > 4)
        {
            possibleTargetPos = target.position;
        }
        mAgent.SetDestination(lastTargetPos);

        if (mAgent.remainingDistance > mAgent.stoppingDistance)// если до цели не дошёл агент
        {
            SetAnimationClip(AnimationsContainer.MoveToPerson);// идти к цели
        }
        else if (mAgent.remainingDistance <= mAgent.stoppingDistance && enemy)// если дошёл
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

    private GameObject gameTarget;
    private void DebugDraw()
    {
        if (!gameTarget)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == "drawer")
                    gameTarget = transform.GetChild(i).gameObject;
            }
        }
        gameTarget.transform.position = lastTargetPos;
        gameTarget.GetComponent<MeshRenderer>().material.color = Color.blue;
        gameTarget.transform.localEulerAngles += new Vector3(0, 1, 0) * Time.fixedDeltaTime;
    }
}
