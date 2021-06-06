using System.Collections.Generic;
using UnityEngine;
using static Inventory.ItemStates;

namespace Inventory
{
    public class PrefabsData
    {
        private readonly Dictionary<ItemsID, InventoryItem> prefabs = new Dictionary<ItemsID, InventoryItem>
            {
                {ItemsID.Axe, Resources.Load<InventoryItem>("InventoryItems\\Axe_Item_1") },
                {ItemsID.TTPistol, Resources.Load<InventoryItem>("InventoryItems\\TT_item_1") },
                { ItemsID.Makarov, Resources.Load<InventoryItem>("InventoryItems\\Makarov_Item_1") },
                { ItemsID.Ak_74, Resources.Load<InventoryItem>("InventoryItems\\AK-74u_Item_1") },
                { ItemsID.CannedFood, Resources.Load<InventoryItem>("InventoryItems\\CannedFood_Item_1") },
                { ItemsID.Milk, Resources.Load<InventoryItem>("InventoryItems\\Milk_Item_1") },
                { ItemsID.Binoculars,Resources.Load<InventoryItem>("InventoryItems\\Binoculars_item_1") },
                { ItemsID.Knife_1,Resources.Load<InventoryItem>("InventoryItems\\Knife_Item_1") },
                { ItemsID.Bullet_7_62,Resources.Load<InventoryItem>("InventoryItems\\7.62 bullet_Item_1") },
                { ItemsID.Bullet_9_27,Resources.Load<InventoryItem>("InventoryItems\\9.27 bullet_Item_1") },
                { ItemsID.Tablets_1,Resources.Load<InventoryItem>("InventoryItems\\Tablets_Item_1") },
                { ItemsID.Lom, Resources.Load<InventoryItem>("InventoryItems\\Lom_Item_1") }
            };
        public InventoryItem GetItemPrefab(int id)=> prefabs[(ItemsID)id];
    }
}