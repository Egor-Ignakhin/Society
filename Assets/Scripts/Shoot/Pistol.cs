using System;
using System.Collections.Generic;
using UnityEngine;
namespace Shoots
{
    public class Pistol : Gun
    {
        protected override void LoadAssets()
        {
            bullet = Resources.Load<Bullet>("Guns\\NotNormal\\9.27 bullet");
            upBullet = Resources.Load<UsedUpBullet>("Guns\\NotNormal\\9.27 bullet(usedUp)");
            fireClip = Resources.Load<AudioClip>("Guns\\MakarovFire");
            startReloadClip = Resources.Load<AudioClip>("Guns\\MakarovTakeOfMag");
            lastReloadClip = Resources.Load<AudioClip>("Guns\\MakarovLastReload");
            nullBulletsClip = Resources.Load<AudioClip>("Guns\\MakarovBulletsNull");
            g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
        protected override void Awake()
        {
            dispenser = new Dispenser(8, 8);
            LoadAssets();
        }
        GameObject g;
        public override float CartridgeDispenser()
        {
            return 0.25f;
        }
        protected override void Update()
        {
            if (Input.GetMouseButtonDown(0))
                Shoot();
            base.Update();
            /*
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit, 10000, layerMask, QueryTriggerInteraction.Ignore))
            {
                var dir = Vector3.Reflect(transform.forward, hit.point);
                if (Physics.Raycast(hit.point, dir, out RaycastHit hit2, 10000, layerMask, QueryTriggerInteraction.Ignore))
                {
                   // g.transform.position = hit2.point;
                }
            }*/
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
        protected override void CreateBullet()
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Bullet newBullet = Instantiate(bullet, spawnBulletPlace.position, spawnBulletPlace.rotation);
            BulletValues bv = new BulletValues(0, maxDistance, caliber, bulletSpeed, 180, Vector3.zero, layerMask);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
            {
                bv.CurrentDistance = hit.distance;
                bv.Angle = Vector3.Angle(transform.position, hit.point);
                bv.PossibleReflectionPoint = Vector3.Reflect(transform.forward, hit.normal);

                Enemy e = null;
                bool enemyFound = hit.transform.parent && hit.transform.parent.TryGetComponent(out e);

                newBullet.Init(bv, hit, enemyFound ? ImpactsContainer.Impacts["Enemy"] : ImpactsContainer.Impacts["Default"], e);          
                return;
            }
            newBullet.Init(bv, ray.GetPoint(maxDistance));
        }
        protected override void PlayFlashEffect()
        {
            flashEffect.Play();
        }
        protected override void DropUsedBullet()
        {
            if (Instantiate(upBullet, droppingPlace.position, droppingPlace.rotation).TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(droppingPlace.right, ForceMode.Impulse);
                rb.AddForce(-droppingPlace.forward, ForceMode.Impulse);
            }
        }
        public override float getRecoilPower()
        {
            return 0.125f;
        }
    }
}