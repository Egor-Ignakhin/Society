using System.Collections;
using System.Collections.Generic;
using Inventory;

namespace SMG
{
    static class GunCharacteristics
    {
        private static readonly Dictionary<int, (int damage, int maxFlyD, int OptFlyD, int Caliber, int DispenserV)> guns;
        static GunCharacteristics()
        {//реализовать реальное описание
            guns = new Dictionary<int, (int damage, int maxFlyD, int OptFlyD, int Caliber, int DispenserV)>
            {
                {ItemStates.MakarovId,(10, 10, 10,10, 10)},
                {ItemStates.Pistol2Id,(20, 20, 20,20, 20)},
                {ItemStates.Ak_74Id,(30, 30, 30,30, 30)}
            };
        }
        public static (int damage, int maxFlyD, int OptFlyD, int Caliber, int DispenserV) GetGunCharacteristics(int id) => guns[id];
    }
}