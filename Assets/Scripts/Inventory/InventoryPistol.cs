using PlayerClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InventoryPistol : InventoryItem
{
    [SerializeField] private float damage;
    private enum typePistols { pistol1, pistol2}
    [SerializeField] private typePistols typePistol;
    protected override void Awake()
    {
        base.Awake();
        SetType(typePistol.ToString());
    }
    public override void Interact(PlayerStatements pl)
    {
        base.Interact(pl);
    }
}
