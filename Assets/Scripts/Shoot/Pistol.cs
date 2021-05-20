using UnityEngine;
namespace Shoots
{
    class Pistol : Gun
    {
        protected override void LoadAssets()
        {
            bullet = Resources.Load<Bullet>("Guns\\NotNormal\\9.27 bullet");
            upBullet = Resources.Load<UsedUpBullet>("Guns\\NotNormal\\9.27 bullet(usedUp)");
            fireClip = Resources.Load<AudioClip>("Guns\\MakarovFire");
            startReloadClip = Resources.Load<AudioClip>("Guns\\MakarovTakeOfMag");
            lastReloadClip = Resources.Load<AudioClip>("Guns\\MakarovLastReload");
            nullBulletsClip = Resources.Load<AudioClip>("Guns\\MakarovBulletsNull");
        }
        protected override void Awake()
        {
            dispenser = new Dispenser(0, 8);
            LoadAssets();
        }
        public override float CartridgeDispenser()
        {
            return 0.25f;
        }
        protected override void Update()
        {
            if (Input.GetMouseButtonDown(0))
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
        public override float GetRecoilPower()
        {
            return 0.125f;
        }
    }
}