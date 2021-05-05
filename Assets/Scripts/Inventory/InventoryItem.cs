using System;
using UnityEngine;


//объект с возможностью положить в инвентарь
public abstract class InventoryItem : InteractiveObject
{
    [SerializeField] protected int count = 1;
    private Inventory.InventoryContainer inventoryContainer;
    public int Id { get; protected set; }
    private void Start()
    {
        inventoryContainer = FindObjectOfType<Inventory.InventoryContainer>();
    }
    public override void Interact(PlayerClasses.PlayerStatements pl)
    {
        inventoryContainer.AddItem(this);
        Destroy(gameObject);
    }
    public int GetCount() => count;
    public void SetId(int id)
    {
        this.Id = id;
        SetDescription();
    }

    internal void SetCount(int c) => count = c;
}
