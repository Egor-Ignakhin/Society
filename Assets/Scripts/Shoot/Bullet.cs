using Society.Enemies;
using Society.Inventory;
using Society.Patterns;

using UnityEngine;
namespace Society.Shoot
{
    /// <summary>
    /// патрон для оружия
    /// </summary>
    public sealed class Bullet : PoolableObject
    {
        private Vector3 target;//точка назначения
        private EnemyCollision enemyCollision;// возможный враг
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
            enemyCollision = e;
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
            {
                Boom();
                mPool.ReturnToPool(this);
            }
        }
        /// <summary>
        /// "взрыв" снаряда
        /// </summary>
        private void Boom()
        {
            if (!haveTarget)// Цели нет?
                return;// выход

            float damage = Gun.GetOptimalDamage(mass, mBv.Speed, area, kf, mBv.CoveredDistance, mBv.MaxDistance);// высчитывание урона

            bulletReceiver?.OnBulletEnter(bulletType);// Есть приёмник пуль? Вызвать его.

            if (enemyCollision)// Есть враг? Ударить его.
                enemyCollision.InjureEnemy(damage);

            else if (BulletValues.CanReflect(mass, kf, mBv.Speed, mBv.StartSpeed, mBv.Angle)
                && Physics.Raycast(target, mBv.PossibleReflectionPoint, out RaycastHit hit, mBv.MaxDistance, mBv.Layers, QueryTriggerInteraction.Ignore))
            {
                hit.transform.TryGetComponent(out enemyCollision);

                mBv.SetValues(hit.distance + mBv.CoveredDistance, Vector3.Reflect(transform.forward, hit.normal), Mathf.Abs(90 - Vector3.Angle(transform.forward, hit.normal)));
                target = hit.point;
                impactEffect.transform.forward = hit.normal;

                reflectSource.PlayOneShot(reflectSound);
                reflectSource.transform.position = hit.point;
                return;
            }

            impactEffect.transform.position = target;

            //Если попали во врага - прикрепляем декаль к нему
            if (enemyCollision)
                impactEffect.transform.SetParent(enemyCollision.transform);


            if (bulletType == BulletType.Electric)
            {
                if (enemyCollision)
                {                    
                    enemyCollision.DebuffEnemy(EnemyDebuffs.SlowingMovement);
                }

                if(bulletReceiver !=  null)
                    Instantiate(electricImpactEffectInstance, bulletReceiver.GetCenter().position, bulletReceiver.GetCenter().rotation, impactEffect.transform.parent);
            }

            impactEffect.SetActive(true);
        }
    }
}