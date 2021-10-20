using Society.Enemies;
using Society.Inventory;
using Society.Patterns;

using System;

using UnityEngine;
namespace Society.Shoot
{
    /// <summary>
    /// патрон для оружия
    /// </summary>
    public sealed class Bullet : PoolableObject
    {
        private Vector3 target;//точка назначения
        private EnemyCollision enemy;// возможный враг
        private bool haveTarget;// имеет ли патрон цель(возможен выстрел в воздух)
        private GameObject impactEffect;// эффекта столкновения

        [SerializeField] private float mass = 0;
        [SerializeField] private float area = 0.649f;
        [SerializeField] private float kf = 1;
        private BulletValues mBv;

        public ItemStates.ItemsID Id;
        private AudioClip reflectSound;
        private AudioSource reflectSource;
        private IBulletReceiver bulletReceiver;
        private BulletType bulletType;
        private GameObject electricImpactEffectInstance;

        public void Init(BulletValues bv, RaycastHit t, GameObject impact, EnemyCollision e, AudioClip rs, AudioSource rsource, IBulletReceiver br, BulletType btp, GameObject eiei)
        {
            mBv = bv;
            target = t.point;
            enemy = e;
            haveTarget = true;
            impactEffect = impact;
            impactEffect = Instantiate(impactEffect);
            impactEffect.transform.forward = t.normal;
            impactEffect.SetActive(false);
            reflectSound = rs;
            reflectSource = rsource;
            bulletReceiver = br;
            bulletType = btp;
            electricImpactEffectInstance = eiei;
        }
        public void Init(BulletValues bv, Vector3 t)
        {
            mBv = bv;
            target = t;
        }

        private void Update()
        {
            if ((transform.position = Vector3.MoveTowards(transform.position, target, mBv.Speed * Time.deltaTime)) == target)
                Boom();
        }
        /// <summary>
        /// "взрыв" снаряда
        /// </summary>
        private void Boom()
        {
            if (haveTarget)
            {
                mBv.SetSpeed();

                float damage = Gun.GetOptimalDamage(mass, mBv.Speed, area, kf, mBv.CoveredDistance, mBv.MaxDistance);
                if (bulletReceiver != null)
                    bulletReceiver.OnBulletEnter(bulletType);

                if (enemy)
                {
                    enemy.InjureEnemy(damage);
                }
                else if (BulletValues.CanReflect(BulletValues.Energy(mass * kf, mBv.Speed), BulletValues.Energy(mass * kf, mBv.StartSpeed), mBv.Speed, mBv.Angle)
                    && Physics.Raycast(target, mBv.PossibleReflectionPoint, out RaycastHit hit, mBv.MaxDistance, mBv.Layers, QueryTriggerInteraction.Ignore))
                {
                    hit.transform.TryGetComponent(out enemy);

                    mBv.SetValues(hit.distance + mBv.CoveredDistance, Vector3.Reflect(transform.forward, hit.normal), Mathf.Abs(90 - Vector3.Angle(transform.forward, hit.normal)));
                    target = hit.point;
                    impactEffect.transform.forward = hit.normal;

                    reflectSource.PlayOneShot(reflectSound);
                    reflectSource.transform.position = hit.point;
                    return;
                }

                impactEffect.transform.position = target;

                //Если попали во врага - прикрепляем декаль к нему
                if (enemy)
                    impactEffect.transform.SetParent(enemy.transform);

                impactEffect.SetActive(true);
                
                if(bulletType == BulletType.Electric)
                {
                    Instantiate(electricImpactEffectInstance, impactEffect.transform.position, impactEffect.transform.rotation, impactEffect.transform.parent);                    

                    if (enemy)
                        enemy.DebuffEnemy(EnemyDebuffs.SlowingMovement);
                }
            }
            mPool.ReturnToPool(this);
        }
    }
}