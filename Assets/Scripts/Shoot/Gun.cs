using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public delegate void RecoilHandler();
    public event RecoilHandler RecoilEvent;
    [SerializeField] protected float damage;
    [SerializeField] protected int ammoCount;
    protected bool possibleShoot;
    public float GetDamage() { return damage; }
    public int GetAmmoCount() { return ammoCount; }
    protected virtual bool Shoot()
    {
        bool canShooting = false;
        if (ammoCount > 0)
        {
            ammoCount--;
            canShooting = true;
            RecoilEvent?.Invoke();
        }
        return canShooting;
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

