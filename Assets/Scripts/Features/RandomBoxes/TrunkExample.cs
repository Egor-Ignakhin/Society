using Inventory;

namespace Features.RandomBoxes
{
    sealed class TrunkExample : RandomlyBox
    {
        protected override void OnInit() => UnlockItems = new System.Collections.Generic.List<ItemStates.ItemsID> { ItemStates.ItemsID.Lom };

        protected override void UpdateType(bool value)
        {
            SetType(value ? "None" : "LockedTrunk");
            SetRotationAtType(value);
        }
    }
}