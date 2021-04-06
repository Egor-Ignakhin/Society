using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : InteractiveObject
{
    [SerializeField] protected int count = 1;
    public override void Interact(PlayerClasses.PlayerStatements pl)
    {
        InventoryContainer.Instance.AddItem(this);
        Destroy(gameObject);
    }
    public int GetCount()
    {
        return count;
    }
}
