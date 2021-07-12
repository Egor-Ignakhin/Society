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
        private static readonly Dictionary<ItemsID, Sprite> sprites = new Dictionary<ItemsID, Sprite>();
        static InventorySpriteData()
        {
            for (int i = 0; i < System.Enum.GetNames(typeof(ItemsID)).Length; i++)            
                sprites.Add((ItemsID)i,Resources.Load<Sprite>($"InventoryItems\\{(ItemsID)i}"));           
        }

        public static Sprite GetSprite(int id) => sprites[(ItemsID)id];
    }
}