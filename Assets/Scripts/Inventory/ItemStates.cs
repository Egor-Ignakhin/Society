using System.Linq;
using System.Collections.Generic;

namespace Inventory
{
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
        public static int GetMaxCount(string type)
        {
            return items[type].maxCount;
        }

        internal static int GetId(string type)
        {
            return items[type].id;
        }
        public static string GetType(int id)
        {
            var myFilteredCollection = items.Where(x => x.Value.id == id);


            foreach (var x in myFilteredCollection)
            {
                return x.Key;
            }
            return null;
        }
    }
    public class NameItems
    {
        public const string DefaultIcon = "DefaultIcon";
        public const string Axe = "Axe";
        public const string Pistol1 = "Pistol1";
        public const string Pistol2 = "Pistol2";
    }
}