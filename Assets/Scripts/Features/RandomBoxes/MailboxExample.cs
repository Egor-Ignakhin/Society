namespace Features.RandomBoxes
{
    sealed class MailboxExample : RandomlyBox
    {
        protected override void OnInit() => UnlockItems = new System.Collections.Generic.List<Inventory.ItemStates.ItemsID> { Inventory.ItemStates.ItemsID.Lom };

        protected override void UpdateType(bool value)
        {
            SetType(value ? "None" : "LockedMailBox");
            SetRotationAtType(value);
        }
    }
}