using Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SMG
{
    public static class ModifierCharacteristics
    {
        public enum GunTitles { None, Makarov, TT_Pistol, Ak_74 }
        public enum ModifierTypes { None, Silencer, Mag, Aim }
        public enum ModifierIndex { None, _1, _2, _3 }

        private static readonly Dictionary<SMGTitleTypeIndex, (string title, string description, Sprite sprite)> modsDescrtiptions 
            = new Dictionary<SMGTitleTypeIndex, (string title, string description, Sprite sprite)>();// словарь содержащий инфу о модификаторе

        //доступ по имени оружия, типу мода и индксу мода и возврат всех возможных характеристик
        public static readonly Dictionary<SMGTitleTypeIndex, SMGModifierItem> modifiersCharacteristics = new Dictionary<SMGTitleTypeIndex, SMGModifierItem>();
        static ModifierCharacteristics()
        {
            modsDescrtiptions = new Dictionary<SMGTitleTypeIndex, (string title, string description, Sprite sprite)>();
            List<SMGTitleTypeIndex> ttis = new List<SMGTitleTypeIndex>();

            var gunsCount = 3;
            for (int g = 1; g <= gunsCount; g++)
            {
                GunTitles gun = (GunTitles)g;
                for (int t = 2; t < 4; t++)
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

        internal static GunTitles GetGunIdFromInvId(int id)
        {
            if (id == (int)ItemStates.ItemsID.Makarov)
                return GunTitles.Makarov;
            if (id == (int)ItemStates.ItemsID.TTPistol)
                return GunTitles.TT_Pistol;
            if (id == (int)ItemStates.ItemsID.Ak_74)
                return GunTitles.Ak_74;
            return GunTitles.None;
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

            internal static SMGTitleTypeIndex StructFromIcGun(SMGInventoryCellGun gun)
            {
                return new SMGTitleTypeIndex((GunTitles)gun.Title, ModifierTypes.Mag, (ModifierIndex)gun.Mag);
            }
        }
        public class SMGModifierItem
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