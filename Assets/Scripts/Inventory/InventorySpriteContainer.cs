using System.Collections.Generic;
using UnityEngine;
namespace Inventory
{
    /// <summary>
    /// класс сод. словарь спрайтов для инвентаря
    /// </summary>
    public static class InventorySpriteContainer
    {
        private static readonly Dictionary<int, Sprite> sprites;
        static InventorySpriteContainer()
        {
            sprites = new Dictionary<int, Sprite> {
            { ItemStates.DefaultId, Resources.Load<Sprite>("InventoryItems/Test_Item_DefaultIcon") },
            { ItemStates.AxeId, Resources.Load<Sprite>("InventoryItems/Axe_1") },
            { ItemStates.MakarovId, Resources.Load<Sprite>("InventoryItems/Makarov_1") },
            { ItemStates.Pistol2Id, Resources.Load<Sprite>("InventoryItems/Test_Item_Pistol2") },
            { ItemStates.Ak_74Id, Resources.Load<Sprite>("InventoryItems/AK-74u_1") },
            { ItemStates.CannedFoodId, Resources.Load<Sprite>("InventoryItems/CannedFood_1") },
            { ItemStates.MilkId, Resources.Load<Sprite>("InventoryItems/Milk_1") }};
        }

        public static Sprite GetSprite(int id) => sprites[id];

    }
}