using Society.Localization;

namespace Society.Inventory
{
    /// <summary>
    /// класс содержащий статы предметов
    /// </summary>
    public static class ItemStates
    {
        #region Constants

        public enum ItemsID : int
        {
            Default, Axe_1, TTPistol, Ak_74, CannedFood, Milk, Binoculars, Knife_1, Bullet_7_62, Bullet_9_27, Tablets_1,
            Lom, WoodBoard, SteelSheet, Brick, Plywood, Rope, Magazine_1, Magazine_2, Magazine_3, Magazine_4, Latchkey, Item
        }
        public enum GunsID : int { TTPistol, Ak_74 }

        #endregion

        /// <summary>
        /// возвращает правду если предмет является оружием
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool ItsGun(int id) => id == (int)ItemsID.TTPistol || id == (int)ItemsID.Ak_74;

        /// <summary>
        /// возвращает максимальное число стака предмета в инвентаре
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public static int GetMaxCount(int id) => LocalizationManager.GetMaxCountItem(id);
        public static decimal GetWeightItem(int id) => LocalizationManager.GetWeightItem(id);
        public static (int food, int water) GetMeatNutrition(int id) => LocalizationManager.GetNutrition(id);

        internal static bool ItsMeal(int id) => id == (int)ItemsID.CannedFood || id == (int)ItemsID.Milk;

        internal static bool ItsMedical(int id) => id == (int)ItemsID.Tablets_1;

        internal static (float health, float radiation) GetMedicalPower(int id) => LocalizationManager.GetMedicalProperties(id);
    }
}