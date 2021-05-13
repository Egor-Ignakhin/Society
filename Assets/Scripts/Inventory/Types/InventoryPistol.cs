using Inventory;
namespace InventoryItems
{
    public sealed class InventoryPistol : InventoryItem
    {        
        protected override void Start()
        {
            base.Start();
            if (Id == 2)
                SetType(ItemStates.MakarovType);
            else if (Id == 4)
                SetType(ItemStates.Ak74Type);
        }
    }
}