using PlayerClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryItems
{
    public sealed class InventoryAxe : InventoryItem
    {
        protected override void Awake()
        {
            base.Awake();
            SetType(InventorySpriteContainer.NameSprites.Axe);
        }
        public override void Interact(PlayerStatements pl)
        {
            base.Interact(pl);
        }
    }
}