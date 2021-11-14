using System;
using System.Collections;

using UnityEngine;

namespace Society.Player
{
    public sealed partial class BasicNeeds : Patterns.Singleton<BasicNeeds>
    {
        #region Fields
        private bool foodWaterMultiply = false;
        public void EnableFoodAndWaterMultiply(bool v) => foodWaterMultiply = v;

        private readonly float waitForThirst = 5f;// частота таймера воды
        private readonly float waitForHunger = 3.5f;// частота таймера еды
        private readonly float waitForRadiation = 4;
        private readonly float regenerationSpeed = 3f;

        private bool BnInitFlag = false;
        private float health;
        public float Health
        {
            get => health;

            private set
            {
                if (EndlessHealth)
                    return;

                if (value < health)// при получении урона
                {
                    timeFromLastDamage = 0;
                }

                value = Mathf.Clamp(value, 0, MaximumHealth);
                if (value == 0 && BnInitFlag)
                    Dead();
                health = value;
                HealthChangeValue?.Invoke((float)Math.Round(health, 0));
            }
        }

        internal void Heal(float h, float r)
        {
            Health += h;
            Radiation -= r;
        }

        internal void SetEnableStamins(bool v) => allStaminsEnable = v;

        private float thirst;
        public float Thirst// вода
        {
            get => thirst;

            private set
            {
                if (EndlessWater)
                    return;
                if (value > MaximumThirst)
                    value = MaximumThirst;
                if (value < 0 && isInitialized)
                    value = 0;

                ThirstChangeValue?.Invoke(thirst = value);
            }
        }

        private float food;
        public float Food// еда
        {
            get => food;

            private set
            {
                if (EndlessFood)
                    return;
                if (value > MaximumFood)
                    value = MaximumFood;
                if (value < 0 && isInitialized)
                    value = 0;

                FoodChangeValue?.Invoke(food = value);
            }
        }

        private float radiation = 0;
        public float Radiation
        {
            get => radiation;
            private set
            {
                radiation = Mathf.Clamp(value, 0, MaximumRadiation);
                RadiationChangeValue?.Invoke(value);
            }
        }

        private readonly float defaultHealth = 30;// изначальное здоровья
        public float MaximumHealth { get; private set; } = 100;// максимальное кол-во здоровья              

        private readonly float defaultThirst = 100;// изначальное кол-во воды
        private readonly float thirstDifference = 1;// количество воды, которое будет отниматься в таймере      
        public float MaximumThirst { get; private set; } = 100;// максимум еды

        private readonly float defaultFood = 200;// изначальное кол-во еды
        private readonly float foodDifference = 1;// количество еды, которое будет отниматься в таймере
        public float MaximumFood { get; private set; } = 200;// максимум еды        

        private readonly float radiationDifference = 1;// количество радиации, которое будет отниматься в таймере        
        private readonly float MaximumRadiation = 3000;// максимум радиации
        private bool isInsadeRadiationZone;
        private int currentCountOfZones;

        public event Action<float> HealthChangeValue;
        public event Action<float> ThirstChangeValue;// событие жажды
        public event Action<float> FoodChangeValue;// событие голодания
        public event Action<float> RadiationChangeValue;// событие голодания   

        private readonly DeadLine deadLine = new DeadLine();

        private readonly float DamageFromRadiation = 1;
        private PlayerCollisionChecked playerCollisionChecked;
        private bool allStaminsEnable = true;
        public static bool EndlessHealth { get; internal set; }
        public static bool EndlessFood { get; internal set; }
        public static bool EndlessWater { get; internal set; }
        public bool PossibleDamgeFromCollision { get; private set; } = true;
        private float timeFromLastDamage;
        #endregion
        private void Start()
        {
            Health = defaultHealth;
            Thirst = defaultThirst;
            Food = defaultFood;
            Radiation = 0;

            playerCollisionChecked = gameObject.AddComponent<PlayerCollisionChecked>();
            playerCollisionChecked.OnInit(this);
            gameObject.AddComponent<PlayerSoundEffects>().Init(this, playerCollisionChecked);
            Init();
            BnInitFlag = true;
        }

        private void OnEnable()
        {
            StartCoroutine(nameof(ThirstTimer));
            StartCoroutine(nameof(HungerTimer));
            StartCoroutine(nameof(RadiationTimer));
            StartCoroutine(nameof(RegenerationTimer));
        }
        /// <summary>
        /// Ранить игрока
        /// </summary>
        /// <param урон="value"></param>
        public void InjurePerson(float value) => Health -= value;
        /// <summary>
        /// ранить игрока и нанести ещё радиации организму
        /// </summary>
        /// <param name="h"></param>
        /// <param name="r"></param>
        public void InjurePerson(float h, float r)
        {
            Health -= h;
            Radiation += r;
        }
        /// <summary>
        /// ранить игрока, нанести радиации организму и возможно, плавно убить
        /// </summary>
        /// <param name="h"></param>
        /// <param name="r"></param>
        /// <param name="time"></param>
        public void InjurePerson(float h, float r, float time)
        {
            Radiation += r;
            if (h >= Health)
            {
                StartCoroutine(KillSlowly(time, Health));
            }
            else
            {
                Health -= h;
            }
        }
        /// <summary>
        /// Постепенное убийство персонажа
        /// </summary>
        /// <param name="time"></param>
        /// <param name="startHealth"></param>
        /// <returns></returns>
        private IEnumerator KillSlowly(float time, float startHealth)
        {
            float frequency = 60;
            float times = time * frequency - 1;
            float deltaHealth = startHealth / times;
            for (int i = 0; i < times; i++)
            {
                Health -= deltaHealth;
                yield return new WaitForSeconds(1 / frequency);
            }
            Health = 0;
            yield break;
        }

        /// <summary>
        /// Функция добавления еды/воды организму персонажа
        /// </summary>
        /// <param name="thirst"></param>
        /// <param name="food"></param>
        public void AddMeal(int thirst, int food)
        {
            Food += food;
            Thirst += thirst;
        }

        public void AddRadiation(float value) => Radiation += value;

        public void RemoveRadiation()
        {
            if (Radiation > 0)
            {
                if (!isInsadeRadiationZone)// радиация снимается в безопасной зоне
                    Radiation -= radiationDifference;
                InjurePerson(DamageFromRadiation);
            }
        }

        public void SetIsInsadeZone(bool value)
        {
            if (value)
            {
                isInsadeRadiationZone = value;
                currentCountOfZones++;
            }
            else
            {
                currentCountOfZones -= 1;
                if (currentCountOfZones == 0)
                    isInsadeRadiationZone = value;
            }
        }
        /// <summary>
        /// функция вызывающая загрузку сцены смерти
        /// </summary>
        private void Dead() => deadLine.LoadDeadScene();

        /// <summary>
        /// Функция регенерации здоровья
        /// </summary>
        private void Regeneration()
        {
            if (Mathf.Approximately(Health, MaximumHealth))
                return;
            //Воды больше 0 и еды больше 0 и радиации нет
            if ((Thirst > 0)
                && (Food > 0)
                && (Radiation <= 0))
            {
                Heal(Time.deltaTime * regenerationSpeed, 0);
                Thirst -= Time.deltaTime;
                Food -= Time.deltaTime;
            }
        }

        private IEnumerator ThirstTimer()
        {
            while (true)
            {
                if (allStaminsEnable)
                {
                    Thirst -= thirstDifference * (foodWaterMultiply ? 2 : 1);
                }
                yield return new WaitForSeconds(waitForThirst);
            }
        }
        private IEnumerator HungerTimer()
        {
            while (true)
            {
                if (allStaminsEnable)
                    Food -= foodDifference * (foodWaterMultiply ? 2 : 1);
                yield return new WaitForSeconds(waitForHunger);
            }
        }
        private IEnumerator RadiationTimer()
        {
            while (true)
            {
                if (allStaminsEnable)
                    RemoveRadiation();
                yield return new WaitForSeconds(waitForRadiation);
            }
        }
        private IEnumerator RegenerationTimer()
        {
            while (true)
            {

                if (allStaminsEnable)
                {
                    if (timeFromLastDamage > 3)
                        Regeneration();
                }
                yield return null;
                timeFromLastDamage += Time.deltaTime;
            }
        }
        internal void SetPossibleDamgeFromCollision(bool v) => PossibleDamgeFromCollision = v;

        #region ForceCommands

        internal static void ForceSetHealth(int value) => Instance.Health = value;
        internal static void ForceSetFood(int value) => Instance.Food = value;
        internal static void ForceSetWater(int value) => Instance.Thirst = value;
        internal static void ForceSetRadiation(int value) => Instance.Radiation = value;

        #endregion
    }
}
