using System;
using System.Collections;
using UnityEngine;

namespace PlayerClasses
{
    public sealed class BasicNeeds : Singleton<BasicNeeds>
    {
        private float waitForThirst = 2;// частота таймера воды
        private float waitForHunger = 2;// частота таймера еды
        private float waitForRadiation = 4;

        private float health;
        public float Health
        {
            get
            {
                return health;
            }
            private set
            {
                if (value > MaximumHealth)
                    value = MaximumHealth;
                if (value <= MinimumHealth)
                {
                    value = MinimumHealth;
                    Dead();
                }
                health = value;
                HealthChangeValue?.Invoke((float)Math.Round(value, 0));
            }
        }

        private int thirst;
        public int Thirst// вода
        {
            get
            {
                return thirst;
            }
            private set
            {
                if (value > MaximumThirst)
                    value = MaximumThirst;
                thirst = value;
                ThirstChangeValue?.Invoke(value);
            }
        }

        private int food;
        public int Food// еда
        {
            get
            {
                return food;
            }
            private set
            {
                if (value > MaximumFood)
                    value = MaximumFood;
                food = value;
                FoodChangeValue?.Invoke(value);
            }
        }

        private float radiation = 1;
        public float Radiation
        {
            get => radiation;
            private set
            {
                if (value > MaximumRadiation) value = MaximumRadiation;
                radiation = value;
                RadiationChangeValue?.Invoke(value);
            }
        }

        private float defaultHealth = 80;// изначальное здоровья
        public float MaximumHealth = 100;// максимальное кол-во здоровья      

        private float MinimumHealth = 0;// минимальное кол-во здоровья

        private int defaultThirst = 100;// изначальное кол-во воды
        private int thirstDifference = 1;// количество воды, которое будет отниматься в таймере
        public int MaximumThirst { get; private set; } = 100;// максимум еды

        private int defaultFood = 200;// изначальное кол-во еды
        private int foodDifference = 1;// количество еды, которое будет отниматься в таймере
        public int MaximumFood { get; private set; } = 200;// максимум еды

        private int defaultRadiation = 0;// изначальное кол-во радиации
        private int radiationDifference = 1;// количество радиации, которое будет отниматься в таймере
        public int MinRadiation = 0;
        private int MaximumRadiation = 3000;// максимум радиации
        private bool isInsadeRadiationZone;
        private int currentCountOfZones;

        public delegate void HealthHandler(float value);
        public event HealthHandler HealthChangeValue;
        public event HealthHandler ThirstChangeValue;// событие жажды
        public event HealthHandler FoodChangeValue;// событие голодания
        public event HealthHandler RadiationChangeValue;// событие голодания   

        private DeadLine deadLine;

        private float DamageFromRadiation = 1;
        private void Start()
        {
            Health = defaultHealth;
            Thirst = defaultThirst;
            Food = defaultFood;
            Radiation = defaultRadiation;

            deadLine = new DeadLine();
            gameObject.AddComponent<PlayerCollisionChecked>().OnInit(this);
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
        public void AddMeal(int thirst, int food)
        {
            Food += food;
            Thirst += thirst;
        }

        public void AddRadiation(float value)
        {
            Radiation += value;
        }
        public void RemoveRadiation()
        {
            if (Radiation > MinRadiation)
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
        private void Dead()
        {
            deadLine.LoadDeadScene();
        }
        private IEnumerator ThirstTimer()
        {
            while (true)
            {
                Thirst -= thirstDifference;
                yield return new WaitForSeconds(waitForThirst);
            }
        }
        private IEnumerator HungerTimer()
        {
            while (true)
            {
                Food -= foodDifference;
                yield return new WaitForSeconds(waitForHunger);
            }
        }
        private IEnumerator RadiationTimer()
        {
            while (true)
            {
                RemoveRadiation();
                yield return new WaitForSeconds(waitForRadiation);
            }
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

        /// <summary>
        /// класс отвечающий за столкновения с объектами
        /// </summary>
        class PlayerCollisionChecked : MonoBehaviour
        {
            private float minValue = 100;// минимальная инерция для счёта урона игроку
            private BasicNeeds bn;
            public void OnInit(BasicNeeds bn)
            {
                this.bn = bn;
            }
            private void OnCollisionEnter(Collision collision)
            {
                float force = 0;
                float mass = 1;
                if (collision.transform.TryGetComponent<Rigidbody>(out var rb))
                {
                    mass = rb.mass;
                }

                for (int i = 0; i < collision.contacts.Length; i++)// итерация по всем точкам соприкосновения
                {
                    float len = Vector3.Project(collision.relativeVelocity,
                                                    collision.contacts[i].normal).magnitude;
                    if (force < len) force = len;
                }

                force = mass * force * force;
                if (force > minValue)// если силв больше минимальной для нанесения урона
                {
                    bn.InjurePerson(force / 10);
                }
            }
        }
    }
}