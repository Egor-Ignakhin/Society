using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shoots
{
    public sealed class Pistol : Gun
    {
        private PlayerClasses.BasicNeeds basicNeeds;
        private Camera cam;
        [SerializeField] private Transform spawnBulletPlace;
        private Bullet bullet;
        private void Awake()
        {
            cam = Camera.main;
            basicNeeds = FindObjectOfType<PlayerClasses.BasicNeeds>();
            bullet = Resources.Load<Bullet>("Guns\\PistolBullet");
        }
        protected override bool Shoot()
        {
            bool canShoot = base.Shoot() && possibleShoot;
            if (!canShoot)
                return canShoot;
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            CreateBullet(ray.direction);
            return canShoot;
        }
        private void TestCreateCube(RaycastHit hit)
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
            g.transform.localScale = Vector3.one * 0.1f;
            g.transform.position = hit.point;
            Destroy(g.GetComponent<BoxCollider>());
        }
        private void CreateBullet(Vector3 target)
        {
            Instantiate(bullet, spawnBulletPlace.position, spawnBulletPlace.rotation).Init(basicNeeds,damage, target);
        }
    }
}