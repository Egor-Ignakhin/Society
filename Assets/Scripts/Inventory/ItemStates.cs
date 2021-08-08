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

        public enum ItemsID : int
        {
            Default, Axe_1, Makarov, TTPistol, Ak_74, CannedFood, Milk, Binoculars, Knife_1, Bullet_7_62, Bullet_9_27, Tablets_1,
            Lom, WoodBoard, SteelSheet, Brick, Plywood, Rope, Magazine_1, Magazine_2, Magazine_3, Magazine_4, Latchkey
        }
        public enum GunsID : int { Makarov, TTPistol, Ak_74 }

        #endregion
      //  private static readonly Dictionary<int, (float health, float radiation)> medicalItems;

        /*static ItemStates()
        {
            medicalItems = new Dictionary<int, (float, float)>
            {
                { (int)ItemsID.Tablets_1, (0, 10)}
            };
        }*/

        /// <summary>
        /// возвращает правду если предмет является оружием
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool ItsGun(int id) => id == (int)ItemsID.Makarov || id == (int)ItemsID.TTPistol || id == (int)ItemsID.Ak_74;

        /// <summary>
        /// возвращает максимальное число стака предмета в инвентаре
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public static int GetMaxCount(int id) => Localization.GetMaxCountItem(id);
        public static decimal GetWeightItem(int id) => Localization.GetWeightItem(id);
        public static (int food, int water) GetMeatNutrition(int id) => Localization.GetNutrition(id);

        internal static bool ItsMeal(int id) => id == (int)ItemsID.CannedFood || id == (int)ItemsID.Milk;

        internal static bool ItsMedical(int id) => id == (int)ItemsID.Tablets_1;

        internal static (float health, float radiation) GetMedicalPower(int id) => Localization.GetMedicalProperties(id);

        internal static bool ItsBullet(int itemId) => (itemId == (int)ItemsID.Bullet_7_62) || (itemId == (int)ItemsID.Bullet_9_27);
    }
}