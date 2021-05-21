using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shoots
{
    /// <summary>
    /// оружие
    /// </summary>
    public abstract class Gun : MonoBehaviour
    {
        public delegate void RecoilHandler();
        public event RecoilHandler RecoilEvent;// событие перезарядки        
        [SerializeField] protected float caliber = 10;// калибр снаряда    
        protected bool possibleShoot = true;// возможность стрелять
        public virtual float CartridgeDispenser() => 1;// возможная частота нажатия на курок в секунду
        private float currentCartridgeDispenser;

        public virtual float ReloadTime() => 5;// время перезарядки
        private float currentReloadTime = 0;
        protected Dispenser dispenser;// магазин
        [SerializeField] private Animator mAnimator;
        public bool IsReload { get; protected set; }// перезаряжается ли оружие
        protected Bullet bullet;// пуля летящая
        protected UsedUpBullet upBullet;// гильза выпадающая

        [SerializeField] protected float bulletSpeed = 315;// стартовая скорость пули
        [SerializeField] protected float maxDistance = 350;// максимальная дистанция поражения


        [SerializeField] protected ParticleSystem flashEffect;// эффект выстрела
        [SerializeField] protected Transform droppingPlace;// затвор
        [SerializeField] protected LayerMask layerMask;
        [SerializeField] protected Transform spawnBulletPlace;// место появление патрона
        [SerializeField] protected AudioSource mAudioSource;
        [SerializeField] protected Inventory.ItemStates.ItemsID bulletId;


        protected AudioClip fireClip;
        protected AudioClip startReloadClip;
        protected AudioClip reloadClip;
        protected AudioClip lastReloadClip;
        protected AudioClip nullBulletsClip;
        private PlayerSoundsCalculator playerSoundsCalculator;

        private bool isAutomatic;// автоматическое ли оружие
        private Inventory.InventoryEventReceiver inventoryEv;

        protected abstract void Awake();
        private void Start()
        {
            playerSoundsCalculator = FindObjectOfType<PlayerSoundsCalculator>();
            inventoryEv = FindObjectOfType<Inventory.InventoryContainer>().EventReceiver;
        }
        protected abstract void LoadAssets();
        protected virtual bool Shoot()
        {
            if (ScreensManager.GetScreen() != null)
                return false;
            bool canShooting = currentCartridgeDispenser >= CartridgeDispenser() && dispenser.CountBullets > 0 && !IsReload;
            if (canShooting)
            {
                currentCartridgeDispenser = 0;
                dispenser.Dispens();
                mAnimator.SetTrigger("Fire");
                mAudioSource.PlayOneShot(fireClip);
            }
            else if (!isAutomatic && currentCartridgeDispenser >= CartridgeDispenser())// если пуль нет, то происходит щелчок пустого затвора
            {
                currentCartridgeDispenser = 0;
                mAudioSource.PlayOneShot(nullBulletsClip);
                isAutomatic = true;
            }
            return canShooting;
        }

        public abstract float GetRecoilPower();

        protected void CallRecoilEvent()
        {
            RecoilEvent?.Invoke();
        }
        protected virtual void Update()
        {
            if (Input.GetMouseButtonUp(0))
                isAutomatic = false;

            CartridgeDispens();

            Reload();

            if (Input.GetKeyDown(KeyCode.R))
            {
                IsReload = true;
            }
        }
        private void CartridgeDispens()
        {
            if (currentCartridgeDispenser < CartridgeDispenser())
                currentCartridgeDispenser += Time.deltaTime;
        }
        private void Reload()
        {
            if (dispenser.IsFull)
                IsReload = false;

            if (!IsReload)
                return;

            int remainingBullets = inventoryEv.Containts(bulletId);

            if (remainingBullets <= 0)
            {
                IsReload = false;
                return;
            }

            IsReload = (currentReloadTime += Time.deltaTime) < ReloadTime();
            mAnimator.SetBool("Reload", IsReload);

            if (!IsReload)
            {
                var outOfRange = remainingBullets - dispenser.maxBullets;
                if(outOfRange > 0)// если патронов в инвентаре больше чем помещается в 1 магазине
                {
                    remainingBullets -= outOfRange;
                }
                inventoryEv.DelItem(bulletId, remainingBullets);                
                dispenser.Reload(remainingBullets);
                currentReloadTime = 0;
            }
        }
        /// <summary>
        /// возвращает оптимальный урон по противнику
        /// </summary>
        /// <param name="G"></param>
        /// <param name="V"></param>
        /// <param name="F"></param>
        /// <param name="S"></param>
        /// <param name="distance"></param>
        /// <param name="maxDistance"></param>
        /// <returns></returns>
        public static float GetOptimalDamage(float G, float V, float F, float S,
            float distance, float maxDistance)
        {
            //mass * speed * area * shape coefficient
            float damage = 0.178f * G * V * F * S;

            if (distance != 0)
                damage /= (distance * 10 / maxDistance);
            // Debug.Log(damage);
            return damage;
        }
        public void PlayStartReloadClip()
        {
            mAudioSource.PlayOneShot(startReloadClip);
        }
        public void PlayReloadSound()
        {
            mAudioSource.PlayOneShot(reloadClip);
        }
        public void PlayLastReloadSound()
        {
            mAudioSource.PlayOneShot(lastReloadClip);
        }


        public void SetPossibleShooting(bool isAnimFinish)
        {
            possibleShoot = isAnimFinish;
        }
        protected abstract void DropUsedBullet();
        protected abstract void PlayFlashEffect();
        protected void CreateBullet()
        {
            Ray ray = GunAnimator.Instance.IsAiming ? Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)) : new Ray(spawnBulletPlace.position, spawnBulletPlace.forward);
            Bullet newBullet = Instantiate(bullet, spawnBulletPlace.position, spawnBulletPlace.rotation);
            BulletValues bv = new BulletValues(0, maxDistance, caliber, bulletSpeed, 180, Vector3.zero, layerMask);
            playerSoundsCalculator.AddNoise(bv.Caliber);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
            {
                bv.SetValues(hit.distance, Vector3.Reflect(transform.forward, hit.normal), Math.Abs(90 - Vector3.Angle(ray.direction, hit.normal)));

                bool enemyFound = hit.transform.TryGetComponent(out EnemyCollision e);

                newBullet.Init(bv, hit, enemyFound ? ImpactsContainer.Impacts["Enemy"] : ImpactsContainer.Impacts["Default"], e);
                return;
            }
            newBullet.Init(bv, ray.GetPoint(maxDistance));
        }

        private void OnDisable()
        {
            //стабилизация перезарядки (обнуление при выключении)
            IsReload = false;
            mAnimator.SetBool("Reload", false);
            currentReloadTime = 0;
        }

        /// <summary>
        /// "магазин" оружия
        /// </summary>
        protected class Dispenser
        {
            public int CountBullets { get; private set; }
            public readonly int maxBullets;
            public Dispenser(int cb, int maxB)
            {
                CountBullets = cb;
                maxBullets = maxB;
            }
            public void Dispens()
            {
                CountBullets--;
            }
            public void Reload(int bulletsCount)
            {
                CountBullets = bulletsCount;
            }
            public bool IsFull => CountBullets == maxBullets;// полна ли обойма
        }

        public static class ImpactsContainer
        {
            public static Dictionary<string, GameObject> Impacts = new Dictionary<string, GameObject> {// эффекты столкновений
                { "Enemy", Resources.Load<GameObject>("WeaponEffects\\Prefabs\\BulletImpactFleshSmallEffect")},
                { "Default", Resources.Load<GameObject>("WeaponEffects\\Prefabs\\BulletImpactStoneEffect")}
        };
        }
    }
}