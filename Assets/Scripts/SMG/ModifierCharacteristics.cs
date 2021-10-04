﻿using Inventory;

using System.Collections.Generic;

using UnityEngine;

namespace SMG
{
    public static class ModifierCharacteristics
    {
        public enum GunTitles { None, TT_Pistol, Ak_74 }
        public enum ModifierTypes { None, Silencer, Mag, Aim }
        public enum ModifierIndex { None, _1, _2, _3 }

        private static readonly Dictionary<SMGTitleTypeIndex, (string title, string description, Sprite sprite)> modsDescrtiptions;

        //доступ по имени оружия, типу мода и индксу мода и возврат всех возможных характеристик
        public static readonly Dictionary<SMGTitleTypeIndex, SMGModifierItem> modifiersCharacteristics = new Dictionary<SMGTitleTypeIndex, SMGModifierItem>();
        static ModifierCharacteristics()
        {
            modsDescrtiptions = new Dictionary<SMGTitleTypeIndex, (string title, string description, Sprite sprite)>();
            List<SMGTitleTypeIndex> ttis = new List<SMGTitleTypeIndex>();

            var gunsCount = 2;
            for (int g = 1; g <= gunsCount; g++)
            {
                GunTitles gun = (GunTitles)g;
                for (int t = 1; t < 4; t++)
                {
                    for (int i = 1; i < 3; i++)
                    {
                        ttis.Add(new SMGTitleTypeIndex(gun, (ModifierTypes)t, (ModifierIndex)i));

                        int index = modifiersCharacteristics.Count;
                        modifiersCharacteristics.Add(ttis[index], new SMGModifierItem(ttis[index], GunCharacteristics.GetBulletsCountFromTTI(ttis[index])));
                        modsDescrtiptions.Add(ttis[index], (GunCharacteristics.GetNormalTitleFromTTI(ttis[index]),
                            GunCharacteristics.GetNormalDescriptionFromTTI(ttis[index]), modifiersCharacteristics[ttis[0]].sprite));
                    }
                }
            }
        }

        internal static (string title, string description, Sprite sprite) GetTitleDescSprite(SMGTitleTypeIndex tti)
        {
            return modsDescrtiptions[tti];
        }
        internal static int GetAmmoCountFromDispenser(int title, int dispenserLevel)
        {
            var key = new SMGTitleTypeIndex((GunTitles)title, ModifierTypes.Mag, (ModifierIndex)dispenserLevel);
            if (modifiersCharacteristics.ContainsKey(key))
                return modifiersCharacteristics[new SMGTitleTypeIndex((GunTitles)title, ModifierTypes.Mag, (ModifierIndex)dispenserLevel)].ammoCount;
            else return 0;
        }
        internal static Sprite GetSprite(SMGTitleTypeIndex modState)
        {
            if (modifiersCharacteristics.ContainsKey(modState))
                return modifiersCharacteristics[modState].sprite;
            else return null;
        }

        /// <summary>
        /// контейнер сод. информацию о типе оружия, типе модификации и качеству модификации
        /// </summary>
        public struct SMGTitleTypeIndex
        {
            public static SMGTitleTypeIndex None { get; } = new SMGTitleTypeIndex(GunTitles.None, ModifierTypes.None, ModifierIndex.None);
            public readonly GunTitles Title;
            public readonly ModifierTypes Type;
            public readonly ModifierIndex Index;
            public SMGTitleTypeIndex(GunTitles gT, ModifierTypes mT, ModifierIndex mI)
            {
                Title = gT;
                Type = mT;
                Index = mI;
            }

            /// <summary>
            /// возвращает информацию об установленном магазине на оружии
            /// </summary>
            /// <param name="gun"></param>
            /// <returns></returns>
            internal static SMGTitleTypeIndex StructFromIcGun(SMGInventoryCellGun gun, ModifierTypes type)
            {
                return new SMGTitleTypeIndex((GunTitles)gun.Title, type, type == ModifierTypes.Mag ?
                    (ModifierIndex)gun.Mag : (type == ModifierTypes.Silencer ? (ModifierIndex)gun.Silencer : (ModifierIndex)gun.Aim));
            }
        }
        /// <summary>
        /// контейнер содержащий спрайт модификатора и его характеристики
        /// </summary>
        public struct SMGModifierItem
        {
            public readonly Sprite sprite;
            public readonly int ammoCount;
            public SMGModifierItem(SMGTitleTypeIndex tti, int aC)
            {
                sprite = Resources.Load<Sprite>($"SMGModifiers\\{tti.Title}\\{tti.Type}\\{tti.Index}");
                ammoCount = aC;
            }
        }
    }
}