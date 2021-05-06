namespace InventoryItems
{
    public sealed class InventoryPistol : InventoryItem
    {        
        protected override void Start()
        {
            base.Start();
            if (Id == 2)
                SetType(nameof(Inventory.NameItems.Makarov));
            else if (Id == 4)
                SetType(nameof(Inventory.NameItems.Ak_74));
        }
    }
}