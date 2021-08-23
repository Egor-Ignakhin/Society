using PlayerClasses;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// наследники класса - враги, их можно убивать. 
/// </summary>
public abstract class Enemy : MonoBehaviour, IMovableController
{
    [SerializeField] protected float fov = 90;// угол обзора монстра
    [SerializeField] protected float seeDistance;// дальность обзора
    [SerializeField] protected float health;// здоровье монстра
    [SerializeField] private float power;
    [SerializeField] private float attackDistance;
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
            get => health;
            set
            {
                if (value <= MinHealth)
                    value = MinHealth;

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
    protected Transform target;// текущая цель
    protected Vector3 lastTargetPos;// последняя позиция которую видел монстр

    internal bool HasEnemy() => enemy != null;

    protected Vector3 possibleTargetPos;// пост-последняя позиция (для поворота в её сторону)

    internal float GetDistanceToTarget() => enemy ? CalculateDistance(target.position) : 100000;

    protected NavMeshAgent mAgent;
    protected Animator mAnim;
    [SerializeField] protected LayerMask layerMasks;
    [SerializeField] private List<Transform> eyes = new List<Transform>();// глаза монстра

    protected BasicNeeds enemy;// текущий противник

    public event System.Action DeathEvent;

    private float WaitTarget;// время которое монстр будет выжидать на последней замеченной позиции игрока    

    public delegate void StepHandler(int physMatIndex, StepSoundData.TypeOfMovement type);
    public event StepHandler EnemyStepEvent;

    private StepSoundData stepSoundData;
    private int CurrentPhysicMaterialIndex;
    private Vector3 oldPos = Vector3.zero;
    private StepEnemy stepEnemy;
    protected AudioClip[] deathClip;
    private TargetPointsManager tpm;
    [SerializeField] private Transform targetPointsParent;
    public bool StepEventIsEnabled { get; set; } = true;
    protected virtual void Start()
    {
        OnInit(attackDistance, power, seeDistance, health);
    }
    private void OnInit(float distanceForAttack, float powerInjure, float seeDistance, float health)
    {
        UVariables = new UniqueVariables(distanceForAttack, powerInjure, seeDistance, health);
        mAgent = GetComponent<NavMeshAgent>();
        mAnim = GetComponent<Animator>();
        stepSoundData = FindObjectOfType<StepSoundData>();
        mAgent.stoppingDistance = UVariables.DistanceForAttack;
        UVariables.ChangeHealthEvent += Death;

        MonstersData.AddEnemy(this);
        stepEnemy = new StepEnemy(this, stepSoundData);
        tpm = new TargetPointsManager(targetPointsParent);

        target = tpm.GetCurrentTarget(this);
        lastTargetPos = target.position;
    }
    protected class AnimationsContainer
    {
        public const string MoveToPerson = "MoveToPerson";
        public const string Death = "Death";
        public const string Attack = "Attack";
    }

    private void FixedUpdate()
    {
        RayCastToEnemy();
        SetPhysMaterial();
        CallStepEvent();
    }
    private void CallStepEvent()
    {
        Vector2 to = new Vector2(transform.position.x, transform.position.z);
        Vector2 from = new Vector2(oldPos.x, oldPos.z);
        StepSoundData.TypeOfMovement type = StepSoundData.TypeOfMovement.None;
        if (Mathf.Abs(Vector2.Distance(to, from)) > Time.fixedDeltaTime * 2)
        {
            if (enemy) type = StepSoundData.TypeOfMovement.Run;
            else type = StepSoundData.TypeOfMovement.Walk;
        }

        EnemyStepEvent?.Invoke(CurrentPhysicMaterialIndex, type);
        oldPos = transform.position;
    }
    /// <summary>
    /// тут вычисляется путь до цели (по корнерам карты)
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private float CalculateDistance(Vector3 pos)
    {
        NavMeshPath path = new NavMeshPath();
        float dist = float.PositiveInfinity;
        if (mAgent.isOnNavMesh && mAgent.CalculatePath(pos, path))
        {
            dist = 0;
            for (int x = 1; x < path.corners.Length; x++)
                dist += Vector3.Distance(path.corners[x - 1], path.corners[x]);
        }
        return dist;
    }
    /// <summary>
    /// тут вычисляется угол обзора по отношению к заданной позиции
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool CalculateAngle(Vector3 pos)
    {
        Vector3 targetDir = pos - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        float borderMin = Mathf.Sin(fov) * (fov / 90) * Mathf.Rad2Deg;
        float borderMax = -Mathf.Sin(fov) * (fov / 90) * Mathf.Rad2Deg;

        bool isIntersected = angle > borderMin && angle < borderMax;

        return isIntersected;

    }
    /// <summary>
    /// бросок линии и возможная схватка с врагом
    /// </summary>
    private void RayCastToEnemy()
    {
        Vector3 pos = BasicNeeds.Instance.transform.position;
        foreach (var e in eyes)
        {
            if (Physics.Linecast(e.position, pos, out RaycastHit hit, layerMasks, QueryTriggerInteraction.Ignore))
            {
                if (CalculateDistance(pos) >= UVariables.SeeDistance)
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
        SetEnemy(null);
    }
    /// <summary>
    /// функция нанесения урона монстром
    /// </summary>
    protected void Attack() => enemy.InjurePerson(UVariables.PowerInjure * Time.deltaTime);

    /// <summary>
    /// функция получения урона монстром
    /// </summary>
    /// <param name="value"></param>
    public virtual void InjureEnemy(float value, bool isPlayerDamage = true)
    {
        if (isPlayerDamage)
            SetEnemy(BasicNeeds.Instance, true);
        UVariables.Health -= value;
    }
    /// <summary>
    /// функция смерти
    /// </summary>
    private void Death(float health)
    {
        if (health > UniqueVariables.MinHealth)
            return;
        mAgent.enabled = false;
        SetAnimationClip();
        mAnim.Play($"death_{Random.Range(1, 3)}");
        mAnim.SetBool(AnimationsContainer.Death, true);
        mAnim.applyRootMotion = true;
        enabled = false;
        DeathEvent.Invoke();
        MonstersData.RemoveEnemy(this);
        PlayDeathClip();
    }
    private void PlayDeathClip()
    {
        stepEnemy.PlayDeathClip(deathClip);
    }

    /// <summary>
    /// функция установки противника
    /// </summary>
    /// <param name="enemy"></param>
    public void SetEnemy(BasicNeeds e, bool fromNoise = false, Transform noiseTarget = null)
    {
        enemy = e;
        SetTarget(enemy ? enemy.transform : tpm.GetCurrentTarget(this));
        if (enemy)
            WaitTarget = 5;
        if (fromNoise)
        {
            enemy = null;
            if (noiseTarget)
                SetTarget(noiseTarget);
        }
    }
    /// <summary>
    /// функция установки цели
    /// </summary>
    /// <param name="target"></param>
    public virtual void SetTarget(Transform t)
    {
        target = t;
        bool possibleMove = CalculateDistance(lastTargetPos) <= UVariables.SeeDistance;
        if (enemy)//враг не потерян
        {
            lastTargetPos = enemy.transform.position;// запись в последнюю известную точку                         
        }
        else if ((mAgent.isOnNavMesh && mAgent.remainingDistance < mAgent.stoppingDistance) || !possibleMove)
        {
            if (WaitTarget < 0 || !possibleMove)
            {
                lastTargetPos = tpm.GetCurrentTarget(this).position;
            }
            else
            {
                WaitTarget -= Time.deltaTime;
                var direction = (possibleTargetPos - transform.position).normalized;
                direction.y = 0f;

                if ((mAgent.steeringTarget - transform.position) != Vector3.zero)
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 1);
            }
        }
        else if (WaitTarget > 4)
        {
            possibleTargetPos = target.position;
        }
        if (mAgent.isOnNavMesh)
            mAgent.SetDestination(lastTargetPos);

        if (mAgent.isOnNavMesh && mAgent.remainingDistance > mAgent.stoppingDistance)// если до цели не дошёл агент
        {
            SetAnimationClip(AnimationsContainer.MoveToPerson);// идти к цели
        }
        else if (mAgent.hasPath && mAgent.remainingDistance <= mAgent.stoppingDistance && enemy)// если дошёл
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

    private void SetPhysMaterial()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit))
            CurrentPhysicMaterialIndex = stepSoundData.GetIndexFromRayCast(hit, transform.position);
        else
            CurrentPhysicMaterialIndex = 0;
    }

    protected void OnDestroy()
    {
        UVariables.ChangeHealthEvent -= Death;
        stepEnemy.OnDestroy();
    }

    public class StepEnemy : StepPlayer
    {
        private Enemy enemy;
        public StepEnemy(IMovableController e, StepSoundData ssd)
        {
            stepSoundData = ssd;

            enemy = (Enemy)e;
            enemy.EnemyStepEvent += OnStep;

            stepPlayerSource = enemy.gameObject.AddComponent<AudioSource>();
            stepPlayerSource.priority = 129;
            stepPlayerSource.spatialBlend = 1;
            stepPlayerSource.pitch = Random.Range(0.95f, 1.05f);
        }
        public void OnDestroy()
        {
            enemy.EnemyStepEvent -= OnStep;
        }

        internal void PlayDeathClip(AudioClip[] deathClip)
        {
            int index = Random.Range(0, deathClip.Length);
            stepPlayerSource.PlayOneShot(deathClip[index]);
        }
    }
    public class TargetPointsManager
    {
        private readonly List<EnemyPoint> points = new List<EnemyPoint>();
        private int currentPointIt = 0;

        public TargetPointsManager(Transform pointsParent)
        {
            points.AddRange(pointsParent.GetComponentsInChildren<EnemyPoint>());
        }
        private void CalculateNextPoint()
        {
            if (points[currentPointIt].GetDelay() > 0)
                return;
            else
            {
                points[currentPointIt].ResetDelay();
            }
            currentPointIt++;
            if (currentPointIt >= points.Count)
                currentPointIt = 0;
        }
        public Transform GetCurrentTarget(Enemy e)
        {
            if (points.Count > 0)
            {
                var dist = e.CalculateDistance(points[currentPointIt].transform.position);
                if (dist < e.attackDistance * 1.5f)
                    CalculateNextPoint();

                return points[currentPointIt].transform;
            }
            else return e.transform;
        }
    }
}
