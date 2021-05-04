using PlayerClasses;
namespace Inventory
{
    namespace InventoryItems
    {
        sealed class InventoryAxe : InventoryItem
        {
            protected override void Awake()
            {
                base.Awake();
                SetId(NameItems.Axe);
                SetType("Axe");
            }
            public override void Interact(PlayerStatements pl)
            {
                base.Interact(pl);
            }
        }
    }
}