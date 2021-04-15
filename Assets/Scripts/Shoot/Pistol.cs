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
        private void Awake()
        {
            basicNeeds = FindObjectOfType<PlayerClasses.BasicNeeds>();
            bullet = Resources.Load<Bullet>("Guns\\PistolBullet");
        }
        protected override bool Shoot()
        {
            bool canShoot = base.Shoot() && possibleShoot;
            if (!canShoot)
                return canShoot;
            CreateBullet();
            return canShoot;
        }
        private void CreateBullet()
        {
            Ray ray = new Ray(transform.position, -transform.right);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
            {
                Instantiate(bullet, spawnBulletPlace.position, spawnBulletPlace.rotation).Init(basicNeeds, damage, hit);
            }
            else
                Instantiate(bullet, spawnBulletPlace.position, spawnBulletPlace.rotation).Init(basicNeeds, damage, ray.GetPoint(maxDistance));
        }
    }
}