using System.Collections.Generic;
using UnityEngine;
using static Inventory.ItemStates;
namespace Inventory
{
    /// <summary>
    /// класс сод. словарь спрайтов для инвентаря
    /// </summary>
    public static class InventorySpriteData
    {
        private static readonly Dictionary<ItemsID, Sprite> sprites;
        static InventorySpriteData()
        {
            sprites = new Dictionary<ItemsID, Sprite> {
            { ItemsID.Default, null},
            { ItemsID.Axe, Resources.Load<Sprite>("InventoryItems\\Axe_1") },
            { ItemsID.Makarov, Resources.Load<Sprite>("InventoryItems\\Makarov_1") },
            { ItemsID.TTPistol, Resources.Load<Sprite>("InventoryItems\\TT_1") },
            { ItemsID.Ak_74, Resources.Load<Sprite>("InventoryItems\\AK-74u_1") },
            { ItemsID.CannedFood, Resources.Load<Sprite>("InventoryItems\\CannedFood_1") },
            { ItemsID.Milk, Resources.Load<Sprite>("InventoryItems\\Milk_1") },
            { ItemsID.Binoculars, Resources.Load<Sprite>("InventoryItems\\Binocular_1") },
            { ItemsID.Knife_1, Resources.Load<Sprite>("InventoryItems\\Knife_1") },
            { ItemsID.Bullet_7_62, Resources.Load<Sprite>("InventoryItems\\7.62 bullet_1") },
            { ItemsID.Bullet_9_27, Resources.Load<Sprite>("InventoryItems\\9.27 bullet_1") },
            { ItemsID.Tablets_1, Resources.Load<Sprite>("InventoryItems\\Tablets_1") },
            { ItemsID.Lom, Resources.Load<Sprite>("InventoryItems\\Lom_1") }
            };
        }

        public static Sprite GetSprite(int id) => sprites[(ItemsID)id];

    }
}