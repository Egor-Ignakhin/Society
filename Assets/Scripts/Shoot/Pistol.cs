using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    private Camera cam;
    private void Awake()
    {
        cam = Camera.main;
    }
    protected override void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(hit.transform.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.InjureEnemy(damage);
            }
        }
    }
}
