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
        [SerializeField] protected int ammoCount;// количество патрон
        [SerializeField] protected float caliber = 10;// калибр снаряда    
        protected bool possibleShoot = true;// возможность стрелять
        public virtual float CartridgeDispenser() => 1;// возможная частота нажатия на курок в секунду
        private float currentCartridgeDispenser;

        public virtual float ReloadTime() => 5;// время перезарядки
        private float currentReloadTime = 0;
        protected Dispenser dispenser;
        [SerializeField] private Animator mAnimator;
        public bool IsReload { get; protected set; }
        protected Bullet bullet;
        protected UsedUpBullet upBullet;

        [SerializeField] protected float bulletSpeed = 315;
        [SerializeField] protected float maxDistance = 350;// максимальная дистанция поражения


        [SerializeField] protected ParticleSystem flashEffect;// эффект выстрела
        [SerializeField] protected Transform droppingPlace;
        [SerializeField] protected LayerMask layerMask;
        [SerializeField] protected Transform spawnBulletPlace;// место появление патрона
        protected abstract void Awake();
        protected abstract void LoadAssets();
        protected virtual bool Shoot()
        {
            bool canShooting = ammoCount > 0 && currentCartridgeDispenser >= CartridgeDispenser() && dispenser.CountBullets > 0 && !IsReload;
            if (canShooting)
            {
                ammoCount--;
                currentCartridgeDispenser = 0;
                dispenser.Dispens();
                mAnimator.SetTrigger("Fire");
            }

            return canShooting;
        }

        public abstract float getRecoilPower();

        protected void CallRecoilEvent()
        {
            RecoilEvent?.Invoke();
        }
        protected virtual void Update()
        {
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

            IsReload = (currentReloadTime += Time.deltaTime) < ReloadTime();
            mAnimator.SetBool("Reload", IsReload);

            if (!IsReload)
            {
                dispenser.Reload();
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


        public void SetPossibleShooting(bool isAnimFinish)
        {
            possibleShoot = isAnimFinish;
        }
        protected abstract void DropUsedBullet();
        protected abstract void PlayFlashEffect();
        protected abstract void CreateBullet();

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
            private readonly int maxBullets;
            public Dispenser(int cb, int maxB)
            {
                CountBullets = cb;
                maxBullets = maxB;
                if (CountBullets == maxBullets)
                    IsFull = true;
            }
            public void Dispens()
            {
                CountBullets--;
                IsFull = false;
            }
            public void Reload()
            {
                CountBullets = maxBullets;
                IsFull = true;
            }
            public bool IsFull { get; private set; }// полна ли обойма
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