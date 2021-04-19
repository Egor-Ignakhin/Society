using System.Collections.Generic;
using UnityEngine;
namespace Shoots
{
    public class Pistol : Gun
    {
        [SerializeField] private Transform spawnBulletPlace;// место появление патрона
        private Bullet bullet;
        private UsedUpBullet upBullet;
        [SerializeField] private LayerMask layerMask;

        [SerializeField] private ParticleSystem flashEffect;// эффект выстрела
        [SerializeField] private Transform droppingPlace;
        private Dictionary<string, GameObject> impacts;// эффекты столкновений
        [SerializeField] private float bulletSpeed = 100;
        [SerializeField] private float maxDistance = 350;// максимальная дистанция поражения
        [SerializeField] private float optimalDistance = 50;// дистанция на которой не теряется убойная сила

        private void LoadAssets()
        {
            bullet = Resources.Load<Bullet>("Guns\\NotNormal\\9.27 bullet");
            upBullet = Resources.Load<UsedUpBullet>("Guns\\NotNormal\\9.27 bullet(usedUp)");
            impacts = new Dictionary<string, GameObject> {

                { "Enemy", Resources.Load<GameObject>("WeaponEffects\\Prefabs\\BulletImpactFleshSmallEffect")},
                { "Default", Resources.Load<GameObject>("WeaponEffects\\Prefabs\\BulletImpactStoneEffect")}
            };
        }
        protected override void Awake()
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
            DropUsedBullet();
            CallRecoilEvent();
            return canShoot;
        }
        /// <summary>
        /// создание пули
        /// </summary>
        private void CreateBullet()
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Bullet newBullet = Instantiate(bullet, spawnBulletPlace.position, spawnBulletPlace.rotation);
            BulletValues bv = new BulletValues(0, maxDistance, caliber, bulletSpeed);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
            {
                bv.currentDistance = hit.distance;
                Enemy e = null;
                bool enemyFound = hit.transform.parent && hit.transform.parent.TryGetComponent(out e);

                newBullet.Init(bv, hit, enemyFound ? impacts["Enemy"] : impacts["Default"], e);

                return;
            }
            newBullet.Init(bv, ray.GetPoint(maxDistance));
        }
        private void PlayFlashEffect()
        {
            flashEffect.Play();
        }
        private void DropUsedBullet()
        {
            if (Instantiate(upBullet, droppingPlace.position, droppingPlace.rotation).TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(droppingPlace.right, ForceMode.Impulse);
                rb.AddForce(-droppingPlace.forward, ForceMode.Impulse);
            }
        }
    }
    struct BulletValues
    {
        public float currentDistance { set; get; }
        public float maxDistance { get; }
        public float caliber { get; }
        public float speed { get; }
        public BulletValues(float currentDistance, float maxDistance, float caliber, float speed)
        {
            this.currentDistance = currentDistance;
            this.maxDistance = maxDistance;
            this.caliber = caliber;
            this.speed = speed;
        }
    }
}