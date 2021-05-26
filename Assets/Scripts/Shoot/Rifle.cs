using UnityEngine;

namespace Shoots
{
    class Rifle : Gun
    {
        protected override void LoadAssets()
        {
            bullet = Resources.Load<Bullet>("Guns\\NotNormal\\7.62 bullet");
            upBullet = Resources.Load<UsedUpBullet>("Guns\\NotNormal\\7.62 bullet (usedup)");
            fireClip = Resources.Load<AudioClip>("Guns\\AkFire");
            reloadClip = Resources.Load<AudioClip>("Guns\\AkReload");
            lastReloadClip = Resources.Load<AudioClip>("Guns\\AkLastReload");
            nullBulletsClip = Resources.Load<AudioClip>("Guns\\MakarovBulletsNull");
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
        public override float GetRecoilPower()
        {
            return 0.125f;
        }
        public override float ReloadTime()
        {
            return 3.75f;
        }
    }
}