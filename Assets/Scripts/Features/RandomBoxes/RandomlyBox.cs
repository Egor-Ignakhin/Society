using Inventory;
using System.Collections.Generic;
using UnityEngine;
namespace Features.RandomBoxes
{
    public abstract class RandomlyBox : InteractiveObject
    {
        [SerializeField] protected List<RandomlyDropeedItem> randomlyDroppedItems = new List<RandomlyDropeedItem>();
        [SerializeField] private Vector3 lockedEulers;
        [SerializeField] private Vector3 openedEulers;
        protected bool IsOpened
        {
            get => isOpened; set
            {
                UpdateType(value);
                isOpened = value;
            }
        }
        private Inventory.InventoryContainer inventoryContainer;
        private bool isOpened;

        private void Start()
        {
            inventoryContainer = FindObjectOfType<InventoryContainer>();
            IsOpened = false;
            OnInit();
        }
        protected abstract void OnInit();
        public override void Interact()
        {
            if (IsOpened)
                return;
            foreach (var possibleItem in UnlockItems)
                if (!inventoryContainer.ContainsId(possibleItem))
                    return;

            int it = Random.Range(0, randomlyDroppedItems.Count);
            inventoryContainer.AddItem((int)randomlyDroppedItems[it].Id, randomlyDroppedItems[it].Count, randomlyDroppedItems[it].Gun);
            IsOpened = true;
        }

        protected List<ItemStates.ItemsID> UnlockItems;

        protected abstract void UpdateType(bool value);
        protected void SetRotationAtType(bool value)
        {
            transform.localEulerAngles = value ? openedEulers : lockedEulers;
        }
        private void OnValidate()
        {
            foreach (var r in randomlyDroppedItems)
            {
                r.Count = Mathf.Clamp(r.Count, 1, Inventory.ItemStates.GetMaxCount((int)r.Id));
            }
        }

    }
    [System.Serializable]
    public class RandomlyDropeedItem
    {
        public Inventory.ItemStates.ItemsID Id;
        public int Count;
        public Inventory.SMGInventoryCellGun Gun;
    }
}