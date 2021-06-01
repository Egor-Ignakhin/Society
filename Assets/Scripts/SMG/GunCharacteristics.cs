using System.Collections.Generic;
using static Inventory.ItemStates;

namespace SMG
{
    static class GunCharacteristics
    {
        private static readonly Dictionary<ItemsID, (string Title, int damage, int maxFlyD, int OptFlyD, int Caliber)> guns;
        static GunCharacteristics()
        {//реализовать реальное описание
            guns = new Dictionary<ItemsID, (string title, int damage, int maxFlyD, int OptFlyD, int Caliber)>
            {
                {ItemsID.Makarov,("Пистолет Макарова (ПМ)",10, 10, 10,10)},
                {ItemsID.TTPistol,("Тульский Токарев обр. 1933г.",20, 20, 20,20)},
                {ItemsID.Ak_74,("Автомат Калашникова 5.5x39 (АК-74M)", 30, 30, 30,30)}
            };
        }
        public static (string title, int damage, int maxFlyD, int OptFlyD, int Caliber) GetGunCharacteristics(int id) => guns[(ItemsID)id];
    }
}