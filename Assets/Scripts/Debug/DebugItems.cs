using Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger
{
    /// <summary>
    /// клас для взятия предметов из дебаггера
    /// </summary>
    class DebugItems : MonoBehaviour, IDebug
    {
        public bool Active { get; set; }
        GameObject IDebug.gameObject => gameObject;
        [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();
        private InventoryContainer inventoryContainer;

        private void Awake()
        {
            inventoryContainer = FindObjectOfType<InventoryContainer>();
        }
        // выдача инвентарю предмета
        public void AddItem(int i) => inventoryContainer.AddItem(items[i]);

        public void Activate()
        {
            Active = true;
            gameObject.SetActive(true);
        }
    }
}