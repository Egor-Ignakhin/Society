using System.Collections.Generic;
using UnityEngine;
namespace Shoots
{
    sealed class Pistol : Gun
    {
        [SerializeField] private Transform spawnBulletPlace;// место появление патрона
        private Bullet bullet;
        [SerializeField] private LayerMask layerMask;
        private float maxDistance = 100;// максимальная дистанция поражения
        [SerializeField] private ParticleSystem flashEffect;// эффект выстрела

        private Dictionary<string, GameObject> impacts;// эффекты столкновений
        private void LoadAssets()
        {
            bullet = Resources.Load<Bullet>("Guns\\PistolBullet");
            impacts = new Dictionary<string, GameObject> {

                { "Enemy", Resources.Load<GameObject>("WeaponEffects\\Prefabs\\BulletImpactFleshSmallEffect")},
                { "Default", Resources.Load<GameObject>("WeaponEffects\\Prefabs\\BulletImpactStoneEffect")}
            };
        }
        private void Awake()
        {
            LoadAssets();
        }
        public override float CartridgeDispenser()
        {
            return 0.25f;
        }
        protected override bool Shoot()
        {
            bool canShoot = base.Shoot() && possibleShoot;
            if (!canShoot)
                return canShoot;
            CreateBullet();
            PlayFlashEffect();
            return canShoot;
        }
        /// <summary>
        /// создание пули
        /// </summary>
        private void CreateBullet()
        {
            Ray ray = new Ray(transform.position, -transform.right);
            Bullet newBullet = Instantiate(bullet, spawnBulletPlace.position, spawnBulletPlace.rotation);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.parent && hit.transform.parent.GetComponent<Enemy>())
                    newBullet.Init(damage, hit, impacts["Enemy"], 75);
                else
                    newBullet.Init(damage, hit, impacts["Default"], 75);
            }
            else
                newBullet.Init(damage, ray.GetPoint(maxDistance), 75);
        }
        private void PlayFlashEffect()
        {
            flashEffect.Play();
        }
    }
}