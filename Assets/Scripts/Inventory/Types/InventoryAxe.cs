namespace Inventory
{
    namespace InventoryItems
    {
        sealed class InventoryAxe : InventoryItem
        {
            protected override void Awake()
            {                           
                SetType(ItemStates.AxeType);
            }
        }
    }
}