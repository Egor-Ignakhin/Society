using System;
using System.Collections.Generic;

namespace Inventory
{
    /// <summary>
    /// класс содержащий статы предметов
    /// </summary>
    class ItemStates
    {
        #region Constants
        public const int DefaultId = 0;
        public const int AxeId = 1;
        public const int MakarovId = 2;
        public const int Pistol2Id = 3;
        public const int Ak_74Id = 4;
        public const int CannedFoodId = 5;
        public const int MilkId = 6;

        private const string AxeName = "Стальной топор";
        private const string MakarovName = "Пистолет Макарова";
        private const string Pistol2Name = "НОНЕ";
        private const string Ak74Name = "Ак-74";
        private const string CannedFoodName = "Консерва";
        private const string MilkName = "Молоко";

        public const string AxeType = "Axe";
        public const string MakarovType = "Makarov";
        public const string Pistol2Type = "НОНЕ";
        public const string Ak74Type = "Ak_74";
        public const string CannedFoodType = "CannedFood";
        public const string MilkType = "Milk";

        #endregion
        private static readonly Dictionary<int, (int maxCount, decimal weight, string title)> items;
        private static readonly Dictionary<int, (int food, int water)> meatItems;

        static ItemStates()
        {
            items = new Dictionary<int, (int, decimal, string)>
            {   {DefaultId, (0,0,"") },
                {AxeId, (1,1.3m,AxeName) },
                {MakarovId, (1,1,MakarovName) },
                {Pistol2Id, (1,1,Pistol2Name) },
                {Ak_74Id, (1,4,Ak74Name) },
                {CannedFoodId,  (7,0.2m,CannedFoodName)},
                {MilkId, (5,0.5m, MilkName) }
            };
            meatItems = new Dictionary<int, (int food, int water)>
            {
                {CannedFoodId,(15,1) },
                {MilkId,(5,19) }
            };
        }

        internal static string GetTitle(int id) => items[id].title;



        /// <summary>
        /// возвращает максимальное число стака предмета в инвентаре
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public static int GetMaxCount(int id) => items[id].maxCount;
        public static decimal GetWeightItem(int id) => items[id].weight;
        public static (int, int) GetMeatNutrition(int id) => meatItems[id];

    }
}