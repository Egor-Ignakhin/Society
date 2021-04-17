using UnityEngine;

//объект с возможностью положить в инвентарь
public abstract class InventoryItem : InteractiveObject
{
    [SerializeField] protected int count = 1;
    public override void Interact(PlayerClasses.PlayerStatements pl)
    {
        Inventory.InventoryContainer.Instance.AddItem(this);
        Destroy(gameObject);
    }
    public int GetCount()
    {
        return count;
    }
}
