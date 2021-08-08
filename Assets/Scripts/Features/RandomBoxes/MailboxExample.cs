using UnityEngine;

namespace Features.RandomBoxes
{
    sealed class MailboxExample : RandomlyBox
    {
        protected override void OnInit()
        {
            UnlockItems = new System.Collections.Generic.List<(int, bool)>
            {
                ((int)Inventory.ItemStates.ItemsID.Lom, false),
                ((int)Inventory.ItemStates.ItemsID.Latchkey, true)
            };
            clipsByIT = new System.Collections.Generic.Dictionary<InveractionType, AudioClip[]>
            {
                { InveractionType.OnOpened,Resources.LoadAll<AudioClip>("Features\\RandomlyBox\\Opened\\MailBox") },
                { InveractionType.OnLocked,Resources.LoadAll<AudioClip>("Features\\RandomlyBox\\Locked\\MailBox") }
            };
        }

        protected override void UpdateType(bool value)
        {
            SetType(value ? "None" : "LockedMailBox");
            SetRotationAtType(value);
        }
    }
}