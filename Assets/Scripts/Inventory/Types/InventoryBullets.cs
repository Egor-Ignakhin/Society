using PlayerClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBullets : InventoryItem
{
    private enum typeBullets { Bullets_12, Bullets_17 }
    [SerializeField] private typeBullets typeBullet;
    protected override void Awake()
    {
        base.Awake();
        SetType(typeBullet.ToString());
    }
    public override void Interact(PlayerStatements pl)
    {
        base.Interact(pl);
    }
}
