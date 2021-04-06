using PlayerClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InventoryItems
{
    public sealed class InventoryPistol : InventoryItem
    {
        [SerializeField] private float damage;

        [SerializeField] private string typePistol;
        protected override void Awake()
        {
            base.Awake();
            SetType(typePistol);
        }
        public override void Interact(PlayerStatements pl)
        {
            base.Interact(pl);
        }
    }
}