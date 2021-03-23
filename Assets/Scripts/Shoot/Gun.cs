using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected int ammoCount;

    public float GetDamage() { return damage; }
    public int GetAmmoCount() { return ammoCount; }
    protected abstract void Shoot();
    protected void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Shoot();
    }
}

