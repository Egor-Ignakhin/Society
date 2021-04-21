using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoots
{
    public class Rifle : Gun
    {
        protected override void Awake()
        {
            dispenser = new Dispenser(30, 30);
            LoadAssets();
        }
        protected override void LoadAssets()
        {
            bullet = Resources.Load<Bullet>("Guns\\NotNormal\\7.62 bullet");
            upBullet = Resources.Load<UsedUpBullet>("Guns\\NotNormal\\7.62 bullet (usedup)");
        }
        protected override void Update()
        {
            if (Input.GetMouseButton(0))
                Shoot();
            base.Update();
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
        protected override void DropUsedBullet()
        {
            if (Instantiate(upBullet, droppingPlace.position, droppingPlace.rotation).TryGetComponent<Rigidbody>(out var rb))
            {
                float m = Random.Range(1, 2);
                rb.AddForce(droppingPlace.right * m, ForceMode.Impulse);
                rb.AddForce(-droppingPlace.forward * m, ForceMode.Impulse);
            }
        }

        protected override void PlayFlashEffect()
        {
            flashEffect.time = 0;
            flashEffect.Play();
        }      
        public override float CartridgeDispenser()
        {
            return 0.125f;
        }
        protected override void CreateBullet()
        {            
            Ray ray = GunAnimator.Instance.isAiming? Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)): new Ray(spawnBulletPlace.position, spawnBulletPlace.forward);
            Bullet newBullet = Instantiate(bullet, spawnBulletPlace.position, spawnBulletPlace.rotation);
            BulletValues bv = new BulletValues(0, maxDistance, caliber, bulletSpeed);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
            {
                bv.CurrentDistance = hit.distance;
                Enemy e = null;
                bool enemyFound = hit.transform.parent && hit.transform.parent.TryGetComponent(out e);

                newBullet.Init(bv, hit, enemyFound ? ImpactsContainer.Impacts["Enemy"] : ImpactsContainer.Impacts["Default"], e);

                return;
            }
            newBullet.Init(bv, ray.GetPoint(maxDistance));
        }
        public override float getRecoilPower()
        {
            return 0.25f;
        }
        public override float ReloadTime()
        {
            return 3.75f;
        }
    }
}