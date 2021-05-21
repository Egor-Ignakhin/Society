using System.Collections.Generic;
using UnityEngine;
namespace Inventory
{
    /// <summary>
    /// класс сод. словарь спрайтов для инвентаря
    /// </summary>
    public static class InventorySpriteData
    {
        private static readonly Dictionary<int, Sprite> sprites;
        static InventorySpriteData()
        {
            sprites = new Dictionary<int, Sprite> {
            { (int)ItemStates.ItemsID.Default, null},
            { (int)ItemStates.ItemsID.Axe, Resources.Load<Sprite>("InventoryItems\\Axe_1") },
            { (int)ItemStates.ItemsID.Makarov, Resources.Load<Sprite>("InventoryItems\\Makarov_1") },
            { (int)ItemStates.ItemsID.TTPistol, Resources.Load<Sprite>("InventoryItems\\Test_Item_Pistol2") },
            { (int)ItemStates.ItemsID.Ak_74, Resources.Load<Sprite>("InventoryItems\\AK-74u_1") },
            { (int)ItemStates.ItemsID.CannedFood, Resources.Load<Sprite>("InventoryItems\\CannedFood_1") },
            { (int)ItemStates.ItemsID.Milk, Resources.Load<Sprite>("InventoryItems\\Milk_1") },
            { (int)ItemStates.ItemsID.Binoculars, Resources.Load<Sprite>("InventoryItems\\Binocular_1") },
            { (int)ItemStates.ItemsID.Knife_1, Resources.Load<Sprite>("InventoryItems\\Knife_1") },
            { (int)ItemStates.ItemsID.Bullet_7_62, Resources.Load<Sprite>("InventoryItems\\7.62 bullet_1") },
            { (int)ItemStates.ItemsID.Bullet_9_27, Resources.Load<Sprite>("InventoryItems\\9.27 bullet_1") },
            { (int)ItemStates.ItemsID.Tablets_1, Resources.Load<Sprite>("InventoryItems\\Tablets_1") }
            };
        }

        public static Sprite GetSprite(int id) => sprites[id];

    }
}