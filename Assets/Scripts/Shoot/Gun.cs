using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected int ammoCount;
    protected bool possibleShoot;
    public float GetDamage() { return damage; }
    public int GetAmmoCount() { return ammoCount; }
    protected virtual bool Shoot()
    {
        if (ammoCount > 0)
        {
            ammoCount--;
            return true;
        }
        return false;
    }
    protected void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    internal void SetPossibleShooting(bool isAnimFinish)
    {
        possibleShoot = isAnimFinish;
    }
}

