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
            { NameItems.Default, Resources.Load<Sprite>("InventoryItems/Test_Item_DefaultIcon") },
            { NameItems.Axe, Resources.Load<Sprite>("InventoryItems/Axe_1") },
            { NameItems.Makarov, Resources.Load<Sprite>("InventoryItems/Makarov_1") },
            { NameItems.Pistol2, Resources.Load<Sprite>("InventoryItems/Test_Item_Pistol2") },
            { NameItems.Ak_74, Resources.Load<Sprite>("InventoryItems/AK-74u_1") },
            { NameItems.CannedFood, Resources.Load<Sprite>("InventoryItems/CannedFood_1") } };
        }

        public static Sprite GetSprite(int id) => sprites[id];

    }
}