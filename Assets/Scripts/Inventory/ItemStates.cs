using System.Collections.Generic;
namespace Inventory
{
    class ItemStates
    {
        private static readonly Dictionary<string, int> items;
        static ItemStates()
        {
            items = new Dictionary<string, int>
            {
                {InventorySpriteContainer.NameSprites.DefaultIcon, 0 },
                {InventorySpriteContainer.NameSprites.Axe, 10 },
                {InventorySpriteContainer.NameSprites.Pistol1, 5 },
                {InventorySpriteContainer.NameSprites.Pistol2, 5 }
            };
        }
        /// <summary>
        /// возвращает максимальное число стака предмета в инвентаре
        /// </summary>
        /// <param type="type"></param>
        /// <returns></returns>
        public static int GetMaxCount(string type)
        {
            return items[type];
        }
    }
}