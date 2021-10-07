using Society.Inventory;
using Society.Player;

using UnityEngine;

namespace Society.Effects.DropableSoundsManager
{
    /// <summary>
    /// класс вызывает шум падения предмета при его столкновении
    /// </summary>
    public sealed class InvItemCollision : MonoBehaviour
    {
        private DropableSoundsManager manager;
        private InventoryItem inventoryItem;
        private Rigidbody mRb;
        private void Start() => manager = FindObjectOfType<DropableSoundsManager>();

        public void OnInit(InventoryItem it, Rigidbody rb)
        {
            inventoryItem = it;
            mRb = rb;
        }
        private void OnCollisionStay(Collision col)
        {
            if (!mRb)
                return;

            if (mRb.velocity.magnitude > 0.5f)
            {
                manager.PlayClip(transform.position, this);
                PlayerSoundsCalculator.AddItemSound(inventoryItem);
            }
        }
    }
}