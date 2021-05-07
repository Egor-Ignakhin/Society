using System.Collections.Generic;

namespace Inventory
{
    /// <summary>
    /// класс содержащий статы предметов
    /// </summary>
    class ItemStates
    {
        private static readonly Dictionary<int, int> items;// id and MaxCount
        private static readonly Dictionary<int, (int food, int water)> meatItems;

        static ItemStates()
        {
            items = new Dictionary<int, int>
            {   {NameItems.Default, 0 },
                {NameItems.Axe, 1 },
                {NameItems.Makarov, 1 },
                {NameItems.Pistol2, 1 },
                {NameItems.Ak_74, 1 },
                {NameItems.CannedFood, 7 },
                {NameItems.Milk, 5 }
            };
            meatItems = new Dictionary<int, (int food, int water)>
            {
                {NameItems.CannedFood,(15,1) },
                {NameItems.Milk,(5,19) }
            };
        }


        /// <summary>
        /// возвращает максимальное число стака предмета в инвентаре
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public static int GetMaxCount(int id) => items[id];
        public static (int, int) GetMeatNutrition(int id)
        {
            return meatItems[id];
        }
    }
    class NameItems
    {
        public const int Default = 0;
        public const int Axe = 1;
        public const int Makarov = 2;
        public const int Pistol2 = 3;
        public const int Ak_74 = 4;
        public const int CannedFood = 5;
        public const int Milk = 6;
    }
}