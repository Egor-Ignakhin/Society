using System.Collections.Generic;

using UnityEngine;

using static Society.Inventory.ItemStates;

namespace Society.Inventory
{
    public class PrefabsData
    {
        private readonly Dictionary<ItemsID, InventoryItem> prefabs = new Dictionary<ItemsID, InventoryItem>();
        public PrefabsData()
        {
            for (int i = 0; i < System.Enum.GetNames(typeof(ItemsID)).Length; i++)
                prefabs.Add((ItemsID)i, Resources.Load<InventoryItem>($"InventoryItems\\{(ItemsID)i}"));
        }
        public InventoryItem GetItemPrefab(int id) => prefabs[(ItemsID)id];
    }
}