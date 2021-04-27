using PlayerClasses;
namespace Inventory
{
    namespace InventoryItems
    {
        public sealed class InventoryAxe : InventoryItem
        {
            protected override void Awake()
            {
                base.Awake();
                SetType(NameItems.Axe);
            }
            public override void Interact(PlayerStatements pl)
            {
                base.Interact(pl);
            }
        }
    }
}