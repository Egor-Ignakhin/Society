using PlayerClasses;
using UnityEngine;
namespace InventoryItems
{
    public sealed class InventoryPistol : InventoryItem
    {
        [SerializeField] private float damage;

        [SerializeField] private int idPistol;
        protected override void Awake()
        {
            base.Awake();
            SetId(idPistol);
        }
        public override void Interact(PlayerStatements pl)
        {
            base.Interact(pl);
        }
    }
}