using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;

namespace SMG
{
    static class GunCharacteristics
    {
        private static readonly Dictionary<int, (string Title, int damage, int maxFlyD, int OptFlyD, int Caliber, int DispenserV)> guns;
        static GunCharacteristics()
        {//реализовать реальное описание
            guns = new Dictionary<int, (string title, int damage, int maxFlyD, int OptFlyD, int Caliber, int DispenserV)>
            {
                {(int)ItemStates.ItemsID.Makarov,("Пистолет Макарова (ПМ)",10, 10, 10,10, 10)},
                {(int)ItemStates.ItemsID.TTPistol,("Тульский Токарев обр. 1933г.",20, 20, 20,20, 20)},
                {(int)ItemStates.ItemsID.Ak_74,("Автомат Калашникова калибра 5,45 мм образца 1974 года(АК-74)", 30, 30, 30,30, 30)}
            };
        }
        public static (string title, int damage, int maxFlyD, int OptFlyD, int Caliber, int DispenserV) GetGunCharacteristics(int id) => guns[id];
    }
}