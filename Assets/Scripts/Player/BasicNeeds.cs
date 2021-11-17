using System;
using System.Collections;

using UnityEngine;

namespace Society.Player
{
    public sealed class BasicNeeds : Patterns.Singleton<BasicNeeds>
    {
        #region Fields
        private bool foodWaterMultiply = false;
        public void EnableFoodAndWaterMultiply(bool v) => foodWaterMultiply = v;

        #region Trackers

        private const float waitForThirst = 5f;//Скорость трекера жажды
        private const float waitForHunger = 3.5f;//Скорость трекера еды
        private const float waitForRadiation = 4;//Скорость трекера радиации
        private const float regenerationSpeed = 3f;//Скорость регенерации

        #endregion
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

                value = Mathf.Clamp(value, 0, MaxHealth);
                if (value == 0 && BnInitFlag)
                    OnDead();
                health = value;
                HealthChangeValue?.Invoke((float)Math.Round(health, 0));
            }
        }

        private float thirst;
        public float Thirst
        {
            get => thirst;

            private set
            {
                if (EndlessWater)
                    return;
                if (value > MaxThirst)
                    value = MaxThirst;
                if (value < 0 && isInitialized)
                    value = 0;

                ThirstChangeValue?.Invoke(thirst = value);
            }
        }

        private float food;
        public float Food
        {
            get => food;

            private set
            {
                if (EndlessFood)
                    return;
                if (value > MaxFood)
                    value = MaxFood;
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
                radiation = Mathf.Clamp(value, 0, MaxRadiation);
                RadiationChangeValue?.Invoke(value);
            }
        }

        public float MaxHealth { get; private set; } = 100;
        public float MaxThirst { get; private set; } = 100;
        public float MaxFood { get; private set; } = 200;

        private const float MaxRadiation = 3000;

        /// <summary>
        /// Игрок в радиоактивной зоне?
        /// </summary>
        private bool isInsideRadiationZone;

        /// <summary>
        /// Активное количество радиоактивных зон
        /// в которых находится игрок
        /// </summary>
        private int currentCountOfZones;

        #region Events

        public event Action<float> HealthChangeValue;
        public event Action<float> ThirstChangeValue;// событие жажды
        public event Action<float> FoodChangeValue;// событие голодания
        public event Action<float> RadiationChangeValue;// событие голодания   

        #endregion

        private readonly PlayerDeathHandler playerDeathHandler = new PlayerDeathHandler();

        private PlayerCollisionChecked playerCollisionChecked;
        private bool allStaminsEnable = true;

        /// <summary>
        /// Урон от коллизий включён?
        /// </summary>
        public bool IsEnableDamageFromCollision { get; private set; } = true;
        private float timeFromLastDamage;
        #endregion
        private void Start()
        {
            Health = MaxHealth;
            Thirst = MaxThirst;
            Food = MaxFood;
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

        internal void Heal(float h, float r)
        {
            Health += h;
            Radiation -= r;
        }

        internal void SetEnableStamins(bool v) => allStaminsEnable = v;

        /// <summary>
        /// Ранить игрока
        /// </summary>
        /// <param урон="value"></param>
        public void InjurePerson(float value) => Health -= value;

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
                if (!isInsideRadiationZone)
                    Radiation--;
                InjurePerson(1);
            }
        }

        public void SetIsInsideZone(bool value)
        {
            if (value)
            {
                isInsideRadiationZone = value;
                currentCountOfZones++;
            }
            else
            {
                currentCountOfZones -= 1;
                if (currentCountOfZones == 0)
                    isInsideRadiationZone = value;
            }
        }
        /// <summary>
        /// функция вызывающая загрузку сцены смерти
        /// </summary>
        private void OnDead() => playerDeathHandler.LoadDeadScene();

        /// <summary>
        /// Функция регенерации здоровья
        /// </summary>
        private void Regeneration()
        {
            if (Mathf.Approximately(Health, MaxHealth))
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


        /// <summary>
        /// Установить возможность получать урон от коллизий
        /// </summary>
        /// <param name="v"></param>
        internal void SetEnableDamageFromCollision(bool v) => IsEnableDamageFromCollision = v;

        #region Coroutines
        private IEnumerator ThirstTimer()
        {
            while (true)
            {
                if (allStaminsEnable)
                {
                    Thirst -=  foodWaterMultiply ? 2 : 1;
                }
                yield return new WaitForSeconds(waitForThirst);
            }
        }
        private IEnumerator HungerTimer()
        {
            while (true)
            {
                if (allStaminsEnable)
                    Food -= foodWaterMultiply ? 2 : 1;
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
        #endregion

        #region ForceProperties

        public static bool EndlessHealth { get; internal set; }
        public static bool EndlessFood { get; internal set; }
        public static bool EndlessWater { get; internal set; }

        #endregion

        #region ForceCommands

        internal static void ForceSetHealth(int value) => Instance.Health = value;
        internal static void ForceSetFood(int value) => Instance.Food = value;
        internal static void ForceSetWater(int value) => Instance.Thirst = value;
        internal static void ForceSetRadiation(int value) => Instance.Radiation = value;

        #endregion
    }
}
