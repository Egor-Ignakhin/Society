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
        public event Action ShootEvent;// событие перезарядки    
        public delegate void DispensetHandler(int remBullets);
        public event DispensetHandler ChangeAmmoCountEvent;
        [SerializeField] protected float caliber = 10;// калибр снаряда    
        protected bool possibleShoot = true;// возможность стрелять
        public float CartridgeDispenser = 1;// возможная частота нажатия на курок в секунду
        private float currentCartridgeDispenser;

        public float ReloadTime;// время перезарядки
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
        protected LayerMask layerMask;
        [SerializeField] protected Transform spawnBulletPlace;// место появление патрона
        [SerializeField] protected Inventory.ItemStates.ItemsID bulletId;

        protected AudioClip fireClip;
        protected AudioClip startReloadClip;
        protected AudioClip reloadClip;
        protected AudioClip lastReloadClip;
        protected AudioClip nullBulletsClip;
        private Effects.PlayerSoundsCalculator playerSoundsCalculator;

        private bool isAutomatic;// автоматическое ли оружие
        protected Inventory.InventoryEventReceiver inventoryEv;
        private Inventory.InventoryContainer InventoryContainer;

        private GunAnimator gunAnimator;
        private EffectsManager effectsManager;
        private SMG.GunModifiersActiveManager gunModifiersActiveManager;
        private UsedUpBulletsDropper ubp;
        private ShootedBulletPool sbp;
        private AudioClip reflectSound;
        private AudioSource reflectSource;
        private void Awake()
        {
            gunModifiersActiveManager = GetComponent<SMG.GunModifiersActiveManager>();
            reflectSound = Resources.Load<AudioClip>("Guns\\BulletReflect");
            reflectSource = new GameObject($"ReflectSource_{GetType()}").AddComponent<AudioSource>();
        }
        private void Start()
        {
            effectsManager = FindObjectOfType<EffectsManager>();
            playerSoundsCalculator = FindObjectOfType<Effects.PlayerSoundsCalculator>();
            InventoryContainer = FindObjectOfType<Inventory.InventoryContainer>();
            inventoryEv = InventoryContainer.EventReceiver;

            dispenser = new Dispenser(inventoryEv);
            LoadAssets();
            (ubp = gameObject.AddComponent<UsedUpBulletsDropper>()).OnInit(upBullet, droppingPlace);
            (sbp = gameObject.AddComponent<ShootedBulletPool>()).OnInit(bullet, spawnBulletPlace);
        }
        internal void OnInit(LayerMask interactableLayers, GunAnimator gAnim)
        {
            layerMask = interactableLayers;
            gunAnimator = gAnim;
        }
        public Inventory.ItemStates.ItemsID GetBulletId() => bulletId;
        protected abstract void LoadAssets();
        protected virtual bool Shoot()
        {
            if (ScreensManager.HasActiveScreen())
                return false;

            if (!possibleShoot)
                return false;

            bool canShooting = currentCartridgeDispenser >= CartridgeDispenser && inventoryEv.GetSelectedCell().MGun.AmmoCount > 0 && !IsReload;
            if (canShooting)
            {
                FastReload(inventoryEv.GetSelectedCell().MGun.AmmoCount);
                currentCartridgeDispenser = 0;
                dispenser.Dispens();
                mAnimator.SetTrigger("Fire");
                gunAnimator.PlayArmorySound(fireClip);
            }
            else if (!isAutomatic && currentCartridgeDispenser >= CartridgeDispenser)// если пуль нет, то происходит щелчок пустого затвора
            {
                currentCartridgeDispenser = 0;
                gunAnimator.PlayArmorySound(nullBulletsClip);
                isAutomatic = true;
            }
            return canShooting;
        }

        public abstract float GetRecoilPower();

        protected void CallRecoilEvent()
        {
            ShootEvent?.Invoke();
            ChangeAmmoCountEvent?.Invoke(dispenser.CountBullets);
        }
        protected virtual void Update()
        {
            if (Input.GetMouseButtonUp(0))
                isAutomatic = false;

            CartridgeDispens();

            Reload();

            if (Input.GetKeyDown(KeyCode.R) && !gunAnimator.IsAiming)
            {
                IsReload = true;
            }
        }
        private void CartridgeDispens()
        {
            if (currentCartridgeDispenser < CartridgeDispenser)
                currentCartridgeDispenser += Time.deltaTime;
        }
        private void Reload()
        {
            if (dispenser.IsFull)
                IsReload = false;


            effectsManager.SetRechargeable(IsReload);

            if (!IsReload)
                return;


            int remainingBullets = inventoryEv.Containts(bulletId);

            if (remainingBullets <= 0)
            {
                IsReload = false;                
                return;
            }

            IsReload = (currentReloadTime += Time.deltaTime) < ReloadTime;
            mAnimator.SetBool("Reload", IsReload);

            if (!IsReload)
            {
                var outOfRange = remainingBullets - dispenser.MaxBullets;
                if (outOfRange > 0)// если патронов в инвентаре больше чем помещается в 1 магазине
                {
                    remainingBullets -= outOfRange;
                }
                inventoryEv.DelItem(bulletId, remainingBullets);
                dispenser.Reload(remainingBullets + inventoryEv.GetSelectedCell().MGun.AmmoCount);
                if (dispenser.CountBullets > dispenser.MaxBullets)
                {
                    InventoryContainer.AddItem((int)bullet.Id, dispenser.CountBullets - dispenser.MaxBullets, null);
                    dispenser.Reload(dispenser.MaxBullets);
                }
                currentReloadTime = 0;
                ChangeAmmoCountEvent?.Invoke(dispenser.CountBullets);
            }
        }

        public void UpdateModifiers(int magIndex, int aimIndex, int silIndex)
        {
            gunModifiersActiveManager.SetMag((SMG.ModifierCharacteristics.ModifierIndex)magIndex);
            gunModifiersActiveManager.SetAim((SMG.ModifierCharacteristics.ModifierIndex)aimIndex);
            gunModifiersActiveManager.SetSilencer((SMG.ModifierCharacteristics.ModifierIndex)silIndex);
        }

        /// <summary>
        /// выполняется при загрузке сохранения
        /// </summary>
        public void FastReload(int remBullets) => dispenser.Reload(remBullets);


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

            if (distance > maxDistance)
                damage = 0;
            // Debug.Log(damage);
            return damage;
        }
        public void PlayStartReloadClip() => gunAnimator.PlayArmorySound(startReloadClip);

        public void PlayReloadSound() => gunAnimator.PlayArmorySound(reloadClip);

        public void PlayLastReloadSound() => gunAnimator.PlayArmorySound(lastReloadClip);

        public void SetPossibleShooting(bool isAnimFinish) => possibleShoot = isAnimFinish;

        protected void DropUsedBullet()
        {
            ubp.Drop();
        }
        public class ShootedBulletPool : ObjectPool
        {
            private Transform spawnBulletPlace;
            public void OnInit(PoolableObject po, Transform sbp)
            {
                spawnBulletPlace = sbp;
                SetPrefabAsset(po);
                Preload();
            }
            public override void SetPrefabAsset(PoolableObject po)
            {
                prefabAsset = po;
            }
            public Bullet CreateBullet()
            {
                var bullet = GetObjectFromPool();
                bullet.transform.SetPositionAndRotation(spawnBulletPlace.position, spawnBulletPlace.rotation);
                return bullet as Bullet;
            }

            protected override int PreLoadedCount()
            {
                return 5;
            }
            protected override bool UnityScale()
            {
                return false;
            }
        }
        public class UsedUpBulletsDropper : ObjectPool
        {
            private Transform droppingPlace;
            public void OnInit(UsedUpBullet up, Transform dp)
            {
                droppingPlace = dp;
                SetPrefabAsset(up);
                Preload();
            }
            public void Drop()
            {
                var bullet = GetObjectFromPool();
                bullet.transform.SetPositionAndRotation(droppingPlace.position, droppingPlace.rotation);
                if (bullet.TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.AddForce(droppingPlace.right * 4, ForceMode.Impulse);
                    rb.AddForce(-droppingPlace.forward * 2, ForceMode.Impulse);
                }
            }
            public override void SetPrefabAsset(PoolableObject instance)
            {
                prefabAsset = instance;
            }
            protected override bool UnityScale()
            {
                return false;
            }

            protected override int PreLoadedCount()
            {
                return 30;
            }
        }
        protected abstract void PlayFlashEffect();
        protected void CreateBullet()
        {
            Ray ray = gunAnimator.IsAiming ? Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)) : new Ray(spawnBulletPlace.position, spawnBulletPlace.forward);
            Bullet newBullet = sbp.CreateBullet();
            BulletValues bv = new BulletValues(0, maxDistance, caliber, bulletSpeed, 180, Vector3.zero, layerMask);
            playerSoundsCalculator.AddNoise(bv.Caliber);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
            {
                bv.SetValues(hit.distance, Vector3.Reflect(transform.forward, hit.normal), Math.Abs(90 - Vector3.Angle(ray.direction, hit.normal)));

                bool enemyFound = hit.transform.TryGetComponent(out EnemyCollision e);
                hit.transform.TryGetComponent(out IBulletReceiver bulletReceiver);
                newBullet.Init(bv, hit, enemyFound ? ImpactsData.Impacts["Enemy"] : ImpactsData.Impacts["Default"], e, reflectSound, reflectSource, bulletReceiver);
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

            if (effectsManager)
                effectsManager.SetRechargeable(IsReload);
        }

        /// <summary>
        /// "магазин" оружия
        /// </summary>
        protected class Dispenser
        {
            public int CountBullets { get; private set; } = 0;
            private readonly Inventory.InventoryEventReceiver inventoryEv;
            public int MaxBullets
            {
                get
                {
                    var sc = inventoryEv.GetSelectedCell();
                    if (sc && Inventory.ItemStates.ItsGun(sc.Id))
                        return SMG.ModifierCharacteristics.GetAmmoCountFromDispenser(sc.MGun.Title, sc.MGun.Mag);
                    return 0;
                }
            }

            public Dispenser(Inventory.InventoryEventReceiver iEv) => inventoryEv = iEv;

            public void Dispens() => CountBullets--;

            public void Reload(int bulletsCount) => CountBullets = bulletsCount;

            public bool IsFull => CountBullets == MaxBullets;// полна ли обойма
            public bool IsEmpty => CountBullets <= 0;
        }

        public static class ImpactsData
        {
            public static Dictionary<string, GameObject> Impacts = new Dictionary<string, GameObject> {// эффекты столкновений
            { "Enemy", Resources.Load<GameObject>("WeaponEffects\\Prefabs\\BulletImpactFleshSmallEffect")},
            { "Default", Resources.Load<GameObject>("WeaponEffects\\Prefabs\\BulletImpactStoneEffect")}
            };
        }
    }
}