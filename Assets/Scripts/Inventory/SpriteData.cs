using System.Collections.Generic;

using UnityEngine;

using static Society.Inventory.ItemStates;

namespace Society.Inventory
{
    /// <summary>
    /// класс сод. словарь спрайтов для инвентаря
    /// </summary>
    public class SpriteData
    {
        private Dictionary<ItemsID, Sprite> sprites = new Dictionary<ItemsID, Sprite>();
        public SpriteData()
        {
            for (int i = 0; i < System.Enum.GetNames(typeof(ItemsID)).Length; i++)
                sprites.Add((ItemsID)i, Resources.Load<Sprite>($"InventoryItems\\{(ItemsID)i}"));
        }

        public Sprite GetSprite(int id) => sprites[(ItemsID)id];
    }
}