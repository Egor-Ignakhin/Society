using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerClasses
{
    public sealed partial class BasicNeeds : Singleton<BasicNeeds>
    {
        private bool foodWaterMultiply = false;
        public void EnableFoodAndWaterMultiply(bool v) => foodWaterMultiply = v;

        private readonly float waitForThirst = 5;// частота таймера воды
        private readonly float waitForHunger = 3.5f;// частота таймера еды
        private readonly float waitForRadiation = 4;

        private float health;
        private bool BnInitFlag = false;
        public float Health
        {
            get => health;

            private set
            {
                if (EndlessHealth)
                    return;
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

        internal void SetEnableStamins(bool v)
        {
            allStaminsEnable = v;
        }

        private int thirst;
        public int Thirst// вода
        {
            get => thirst;

            private set
            {
                if (EndlessWater)
                    return;
                if (value > MaximumThirst)
                    value = MaximumThirst;
                if (value < 0 && isInitialized)
                {
                    InjurePerson(-value);
                    value = 0;
                }
                ThirstChangeValue?.Invoke(thirst = value);
            }
        }

        private int food;
        public int Food// еда
        {
            get => food;

            private set
            {
                if (EndlessFood)
                    return;
                if (value > MaximumFood)
                    value = MaximumFood;
                if (value < 0 && isInitialized)
                {
                    InjurePerson(-value);
                    value = 0;
                }
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

        private readonly float defaultHealth = 80;// изначальное здоровья
        public float MaximumHealth = 100;// максимальное кол-во здоровья              

        private readonly int defaultThirst = 100;// изначальное кол-во воды
        private readonly int thirstDifference = 1;// количество воды, которое будет отниматься в таймере      
        public int MaximumThirst { get; private set; } = 100;// максимум еды

        private readonly int defaultFood = 200;// изначальное кол-во еды
        private readonly int foodDifference = 1;// количество еды, которое будет отниматься в таймере
        public int MaximumFood { get; private set; } = 200;// максимум еды        

        private readonly int radiationDifference = 1;// количество радиации, которое будет отниматься в таймере        
        private readonly int MaximumRadiation = 3000;// максимум радиации
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
        }
        public void InjurePerson(float value)
        {
            Health -= value;
        }
        public void InjurePerson(float h, float r)
        {
            Health -= h;
            Radiation += r;
        }
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
        IEnumerator KillSlowly(float time, float startHealth)
        {
            float frequency = 60;
            float times = time * frequency - 1;
            float deltaHealth = startHealth / times;
            for (int i = 0; i < times; i++)
            {
                Health -= deltaHealth;
                yield return new WaitForSeconds(1/frequency);
            }
            Health = 0;
            yield break;
        }
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

        private void Regeneration()
        {
            if (Thirst > MaximumThirst / 2 && Food > MaximumFood / 2 && Radiation <= 0)
            {
                Heal(1, 0);
                Thirst--;
                Food--;
            }
        }
        private IEnumerator ThirstTimer()
        {
            while (true)
            {
                if (allStaminsEnable)
                {
                    Thirst -= thirstDifference * (foodWaterMultiply ? 2 : 1);
                    Regeneration();
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
        internal void SetPossibleDamgeFromCollision(bool v)
        {
            PossibleDamgeFromCollision = v;
        }


        #region ForceCommands

        internal static void ForceSetHealth(int value)
        {
            Instance.Health = value;
        }
        internal static void ForceSetFood(int value)
        {
            Instance.Food = value;
        }
        internal static void ForceSetWater(int value)
        {
            Instance.Thirst = value;
        }
        internal static void ForceSetRadiation(int value)
        {
            Instance.Radiation = value;
        }

        #endregion
    }
}
