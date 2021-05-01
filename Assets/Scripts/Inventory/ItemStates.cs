using System.Linq;
using System.Collections.Generic;

namespace Inventory
{
    /// <summary>
    /// класс содержащий статы предметов
    /// </summary>
    class ItemStates
    {
        private static readonly Dictionary<string, (int maxCount, int id)> items;

        static ItemStates()
        {
            items = new Dictionary<string, (int maxCount, int id)>
            {
                {NameItems.DefaultIcon, (0,0) },
                {NameItems.Axe, (10 ,1)},
                {NameItems.Pistol1, (5,2) },
                {NameItems.Pistol2, (5,3) }
            };
        }
        /// <summary>
        /// возвращает максимальное число стака предмета в инвентаре
        /// </summary>
        /// <param type="type"></param>
        /// <returns></returns>
        public static int GetMaxCount(string type) => items[type].maxCount;

        /// <summary>
        /// возвращает id по типу (строка)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static int GetId(string type) => items[type].id;

        public static string GetType(int id)
        {
            var item = items.Where(x => x.Value.id == id).First().Key;
            
            return item;
        }
    }
    class NameItems
    {
        public const string DefaultIcon = "DefaultIcon";
        public const string Axe = "Axe";
        public const string Pistol1 = "Pistol1";
        public const string Pistol2 = "Pistol2";
    }
}