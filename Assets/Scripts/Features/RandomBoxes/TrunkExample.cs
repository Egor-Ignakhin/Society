using UnityEngine;

namespace Society.Features.RandomBoxes
{
    internal sealed class TrunkExample : RandomlyBox
    {
        protected override void OnInit()
        {
            UnlockItems = new System.Collections.Generic.List<(int, bool)> { ((int)Society.Inventory.ItemStates.ItemsID.Lom, false) };
            clipsByIT = new System.Collections.Generic.Dictionary<InveractionType, AudioClip[]>
            {
                { InveractionType.OnOpened,Resources.LoadAll<AudioClip>("Features\\RandomlyBox\\Opened\\Trunk") },
                { InveractionType.OnLocked,Resources.LoadAll<AudioClip>("Features\\RandomlyBox\\Locked\\Trunk") }
            };
        }

        protected override void UpdateType(bool value)
        {
            SetType(value ? "None" : "LockedTrunk");
            SetRotationAtType(value);
        }
    }
}