using System;
using System.Collections.Generic;

namespace Inventory
{
    /// <summary>
    /// класс содержащий статы предметов
    /// </summary>
    public static class ItemStates
    {
        #region Constants

        public enum ItemsID : int { Default, Axe, Makarov, TTPistol, Ak_74, CannedFood, Milk, Binoculars, Knife_1, Bullet_7_62, Bullet_9_27, Tablets_1 }

        #endregion
        private static readonly Dictionary<int, (int maxCount, decimal weight)> items;
        private static readonly Dictionary<int, (int food, int water)> meatItems;
        private static readonly Dictionary<int, (float health, float radiation)> medicalItems;

        static ItemStates()
        {
            items = new Dictionary<int, (int, decimal)>
            {   {(int)ItemsID.Default, (0,0) },
                {(int)ItemsID.Axe, (1,1.3m) },
                {(int)ItemsID.Makarov, (1,1) },
                {(int)ItemsID.TTPistol, (1,1) },
                {(int)ItemsID.Ak_74, (1,4) },
                {(int)ItemsID.CannedFood,  (7,0.2m)},
                {(int)ItemsID.Milk, (5,0.5m) },
                {(int)ItemsID.Binoculars, (1,0.5m) },
                {(int)ItemsID.Knife_1, (1,0.5m) },
                {(int)ItemsID.Bullet_7_62, (30,0.05m) },
                {(int)ItemsID.Bullet_9_27, (30,0.05m) },
                {(int)ItemsID.Tablets_1, (9,0.005m) }
            };
            meatItems = new Dictionary<int, (int food, int water)>
            {
                {(int)ItemsID.CannedFood,(15,1) },
                {(int)ItemsID.Milk,(5,19) }
            };
            medicalItems = new Dictionary<int, (float,float)>
            {
                { (int)ItemsID.Tablets_1, (0, 10)}
            };
        }

        /// <summary>
        /// возвращает правду если предмет является оружием
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static bool ItsGun(int id) => id == (int)ItemsID.Makarov || id == (int)ItemsID.TTPistol || id == (int)ItemsID.Ak_74;


        /// <summary>
        /// возвращает максимальное число стака предмета в инвентаре
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public static int GetMaxCount(int id) => items[id].maxCount;
        public static decimal GetWeightItem(int id) => items[id].weight;
        public static (int, int) GetMeatNutrition(int id) => meatItems[id];

        internal static bool ItsMeal(int id) => id == (int)ItemsID.CannedFood || id == (int)ItemsID.Milk;

        internal static bool ItsMedical(int id) => id == (int)ItemsID.Tablets_1;

        internal static (float health , float radiation) GetMedicalPower(int id) => medicalItems[id];

    }
}