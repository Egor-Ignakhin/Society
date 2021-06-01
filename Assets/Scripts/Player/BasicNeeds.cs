using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerClasses
{
    public sealed class BasicNeeds : Singleton<BasicNeeds>
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
                value = Mathf.Clamp(value, 0, MaximumHealth);
                if (value == 0 && BnInitFlag)
                    Dead();

                HealthChangeValue?.Invoke((float)Math.Round(health = value, 0));
            }
        }

        internal void Heal(float h, float r)
        {
            Health += h;
            Radiation -= r;
        }

        private int thirst;
        public int Thirst// вода
        {
            get => thirst;

            private set
            {
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

        public delegate void HealthHandler(float value);
        public event HealthHandler HealthChangeValue;
        public event HealthHandler ThirstChangeValue;// событие жажды
        public event HealthHandler FoodChangeValue;// событие голодания
        public event HealthHandler RadiationChangeValue;// событие голодания   

        private readonly DeadLine deadLine = new DeadLine();

        private readonly float DamageFromRadiation = 1;
        private PlayerCollisionChecked playerCollisionChecked;
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
        public void InjurePerson(float value) => Health -= value;
        public void InjurePerson(float h, float r)
        {
            Health -= h;
            Radiation += r;
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
                Thirst -= thirstDifference * (foodWaterMultiply ? 2 : 1);
                Regeneration();
                yield return new WaitForSeconds(waitForThirst);
            }
        }
        private IEnumerator HungerTimer()
        {
            while (true)
            {
                Food -= foodDifference * (foodWaterMultiply ? 2 : 1);
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
    }
    /// <summary>
    /// класс отвечающий за столкновения с объектами
    /// </summary>
    class PlayerCollisionChecked : MonoBehaviour
    {
        private readonly float minValue = 100;// минимальная инерция для счёта урона игроку
        private BasicNeeds bn;
        public delegate void CollisionContactHandler();
        public event CollisionContactHandler PlayerTakingDamageEvent;
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
            if (force > minValue)// если сила больше минимальной для нанесения урона
            {
                bn.InjurePerson(force / 10);
                PlayerTakingDamageEvent?.Invoke();
            }
        }
    }

    class PlayerSoundEffects : MonoBehaviour
    {
        private const float minHealthForNoise = 20;
        private BasicNeeds basicNeeds;
        private AudioClip noiseClip;
        private AudioSource noiseSource;
        private AudioSource playerSong;
        private bool wasDamaged = false;
        private bool coroutineStarted = false;
        private PlayerCollisionChecked playerCollisionChecked;
        private readonly List<AudioClip> vulnerableCollisionClips = new List<AudioClip>();
        public void Init(BasicNeeds bn, PlayerCollisionChecked pcc)
        {
            basicNeeds = bn;
            noiseClip = Resources.Load<AudioClip>("Health\\Shum_Low_Health");

            noiseSource = bn.gameObject.AddComponent<AudioSource>();
            playerSong = bn.gameObject.AddComponent<AudioSource>();
            playerSong.priority = 127;

            basicNeeds.HealthChangeValue += ChangeHealth;
            noiseSource.clip = noiseClip;
            noiseSource.volume = 0;
            noiseSource.loop = true;
            ChangeHealth(basicNeeds.Health);
            noiseSource.Play();

            playerCollisionChecked = pcc;
            playerCollisionChecked.PlayerTakingDamageEvent += OnPlayerVulnerableCollision;
            vulnerableCollisionClips.Add(Resources.Load<AudioClip>("Health\\ablat"));
            vulnerableCollisionClips.Add(Resources.Load<AudioClip>("Health\\bolnovnoge"));
            vulnerableCollisionClips.Add(Resources.Load<AudioClip>("Health\\shivi_shive"));
            vulnerableCollisionClips.Add(Resources.Load<AudioClip>("Health\\hma_hma"));
        }
        private void OnDisable()
        {
            basicNeeds.HealthChangeValue -= ChangeHealth;
            playerCollisionChecked.PlayerTakingDamageEvent -= OnPlayerVulnerableCollision;
        }
        /// <summary>
        /// вызов при столкневии, падении игрока (с игроком)
        /// </summary>
        private void OnPlayerVulnerableCollision()
        {
            AudioClip CalculateAudio()
            {
                //цикл выполняется покуда не найдёт трек, который не является текущим
                int index;
                do
                {
                    index = UnityEngine.Random.Range(0, vulnerableCollisionClips.Count);
                }
                while (vulnerableCollisionClips[index] == playerSong.clip);
                return vulnerableCollisionClips[index];
            }
            playerSong.PlayOneShot(CalculateAudio());
        }
        private void ChangeHealth(float v)
        {
            //хп больше минимального выход
            if (v > minHealthForNoise)
            {
                if (wasDamaged && !coroutineStarted)
                {
                    StartCoroutine(nameof(ClipSilencer));
                    coroutineStarted = true;
                    wasDamaged = false;
                }
                return;
            }
            noiseSource.volume = minHealthForNoise / (v * 50);
            wasDamaged = true;
        }
        private IEnumerator ClipSilencer()
        {
            while (true)
            {
                noiseSource.volume = Mathf.Lerp(noiseSource.volume, 0.001f, Time.deltaTime * 0.1f);
                if (noiseSource.volume < 0.005f)
                {
                    StopCoroutine(nameof(ClipSilencer));
                    coroutineStarted = false;
                    noiseSource.volume = 0;
                }
                yield return null;
            }
        }
    }

}
