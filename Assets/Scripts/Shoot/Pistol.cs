using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shoots
{
    public sealed class Pistol : Gun
    {
        private PlayerClasses.BasicNeeds basicNeeds;
        [SerializeField] private Transform spawnBulletPlace;
        private Bullet bullet;
        [SerializeField] private LayerMask layerMask;
        private float maxDistance = 100;
        [SerializeField] private ParticleSystem flashEffect;

        private Dictionary<string, GameObject> impacts = new Dictionary<string, GameObject>();
        private void LoadAssets()
        {
            bullet = Resources.Load<Bullet>("Guns\\PistolBullet");

            impacts.Add("Enemy", Resources.Load<GameObject>("WeaponEffects\\Prefabs\\BulletImpactFleshSmallEffect"));
            impacts.Add("Default", Resources.Load<GameObject>("WeaponEffects\\Prefabs\\BulletImpactStoneEffect"));
        }
        private void Awake()
        {
            basicNeeds = FindObjectOfType<PlayerClasses.BasicNeeds>();
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
        private void CreateBullet()
        {
            Ray ray = new Ray(transform.position, -transform.right);
            Bullet newBullet;
            newBullet = Instantiate(bullet, spawnBulletPlace.position, spawnBulletPlace.rotation);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.parent && hit.transform.parent.GetComponent<Enemy>())
                    newBullet.Init(basicNeeds, damage, hit, impacts["Enemy"]);
                else
                    newBullet.Init(basicNeeds, damage, hit, impacts["Default"]);
            }
            else
                newBullet.Init(basicNeeds, damage, ray.GetPoint(maxDistance));
        }
        private void PlayFlashEffect()
        {
            flashEffect.Play();
        }
    }
}