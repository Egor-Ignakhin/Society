using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger
{
    public class DebugItems : MonoBehaviour, IDebug
    {
        public bool Active { get; set; }
        GameObject IDebug.gameObject => gameObject;
        [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();

        public void AddItem(int i)
        {
            InventoryContainer.Instance.AddItem(items[i]);
        }

        public void Activate()
        {
            Active = true;
            gameObject.SetActive(true);
        }
    }
}