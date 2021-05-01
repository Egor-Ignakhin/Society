using System.Collections.Generic;
using UnityEngine;
namespace Inventory
{
    /// <summary>
    /// класс сод. словарь спрайтов для инвентаря
    /// </summary>
    public static class InventorySpriteContainer
    {
        private static readonly Dictionary<string, Sprite> sprites;
        static InventorySpriteContainer()
        {
            sprites = new Dictionary<string, Sprite> {
            { NameItems.DefaultIcon, Resources.Load<Sprite>("InventoryItems/Test_Item_DefaultIcon") },
            { NameItems.Axe, Resources.Load<Sprite>("InventoryItems/Test_Item_Axe") },
            { NameItems.Pistol1, Resources.Load<Sprite>("InventoryItems/Test_Item_Pistol1") },
            { NameItems.Pistol2, Resources.Load<Sprite>("InventoryItems/Test_Item_Pistol2") } };
        }
        public static Sprite GetSprite(string type) => sprites[type];

    }
}