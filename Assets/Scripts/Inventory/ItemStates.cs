using System.Collections.Generic;
using System.Linq;

namespace Inventory
{
    /// <summary>
    /// класс содержащий статы предметов
    /// </summary>
    class ItemStates
    {
        private static readonly Dictionary<int, int> items;// id and MaxCount

        static ItemStates()
        {
            items = new Dictionary<int, int>
            {   {NameItems.Default, 0 },
                {NameItems.Axe, 10 },
                {NameItems.Pistol1, 5 },
                {NameItems.Pistol2, 5 }
            };
        }


        /// <summary>
        /// возвращает максимальное число стака предмета в инвентаре
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public static int GetMaxCount(int id) => items[id];
    }
    class NameItems
    {
        public const int Default = 0;
        public const int Axe = 1;
        public const int Pistol1 = 2;
        public const int Pistol2 = 3;
    }
}