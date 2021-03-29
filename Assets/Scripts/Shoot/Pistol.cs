using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Pistol : Gun
{
    private Camera cam;
    private Vector3 rayStartPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    private void Awake()
    {
        cam = Camera.main;
    }
    protected override bool Shoot()
    {
        bool canShoot = base.Shoot();
        if (!canShoot)
            return canShoot;
        Ray ray = cam.ScreenPointToRay(rayStartPos);


        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            TestCreateCube(hit);

            if (hit.transform.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.InjureEnemy(damage);
            }
        }
        return canShoot;
    }
    private void TestCreateCube(RaycastHit hit)
    {
        GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.transform.localScale = Vector3.one * 0.1f;
        g.transform.position = hit.point;
        Destroy(g.GetComponent<BoxCollider>());
    }
}
