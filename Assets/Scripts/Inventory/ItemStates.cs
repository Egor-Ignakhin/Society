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
                {NameItems.DefaultIcon, 0 },
                {NameItems.Axe, 10 },
                {NameItems.Pistol1, 5 },
                {NameItems.Pistol2, 5 }
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
    public class NameItems
    {
        public const string DefaultIcon = "DefaultIcon";
        public const string Axe = "Axe";
        public const string Pistol1 = "Pistol1";
        public const string Pistol2 = "Pistol2";
    }
}