using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    private Camera cam;
    private Vector3 rayStartPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    private void Awake()
    {
        cam = Camera.main;
    }
    protected override void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(rayStartPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {            
            if (hit.transform.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.InjureEnemy(damage);
            }
        }
    }
}
