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
            if(i == 0)
            {

            }
            else if (i == 1)
            {
                InventoryContainer.Instance.AddItem(items[i]);
            }
        }
    }
}