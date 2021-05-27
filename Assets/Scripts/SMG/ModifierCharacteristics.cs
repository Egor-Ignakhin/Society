using System;
using System.Collections.Generic;
using UnityEngine;

namespace SMG
{
    public static class ModifierCharacteristics
    {
        public enum GunTitles { None, Makarov, TT_Pistol, Ak_74 }
        public enum ModifierTypes { None, Silencer, Mag }
        public enum ModifierIndex { None, _1, _2, _3 }

        private static readonly Dictionary<SMGTitleTypeIndex, (string title, string description, Sprite sprite)> modsDescrtiptions;// словарь содержащий инфу о модификаторе

        //доступ по имени оружия, типу мода и индксу мода и возврат всех возможных характеристик
        public static readonly Dictionary<SMGTitleTypeIndex, SMGModifierItem> modifiersCharacteristics = new Dictionary<SMGTitleTypeIndex, SMGModifierItem>();
        static ModifierCharacteristics()
        {
            SMGTitleTypeIndex Makarov_Mag_None = new SMGTitleTypeIndex(GunTitles.Makarov, ModifierTypes.Mag, ModifierIndex.None);
            SMGTitleTypeIndex Makarov_Mag_1 = new SMGTitleTypeIndex(GunTitles.Makarov, ModifierTypes.Mag, ModifierIndex._1);
            SMGTitleTypeIndex Makarov_Mag_2 = new SMGTitleTypeIndex(GunTitles.Makarov, ModifierTypes.Mag, ModifierIndex._2);
            modifiersCharacteristics.Add(Makarov_Mag_None, new SMGModifierItem(Makarov_Mag_None, 0, 0));
            modifiersCharacteristics.Add(Makarov_Mag_1, new SMGModifierItem(Makarov_Mag_1, 7, 0));
            modifiersCharacteristics.Add(Makarov_Mag_2, new SMGModifierItem(Makarov_Mag_2, 9, 0));

            SMGTitleTypeIndex TT_Mag_None = new SMGTitleTypeIndex(GunTitles.TT_Pistol, ModifierTypes.Mag, ModifierIndex.None);
            SMGTitleTypeIndex TT_Mag_1 = new SMGTitleTypeIndex(GunTitles.TT_Pistol, ModifierTypes.Mag, ModifierIndex._1);
            SMGTitleTypeIndex TT_Mag_2 = new SMGTitleTypeIndex(GunTitles.TT_Pistol, ModifierTypes.Mag, ModifierIndex._2);
            modifiersCharacteristics.Add(TT_Mag_None, new SMGModifierItem(TT_Mag_None, 0, 0));
            modifiersCharacteristics.Add(TT_Mag_1, new SMGModifierItem(TT_Mag_1, 6, 0));
            modifiersCharacteristics.Add(TT_Mag_2, new SMGModifierItem(TT_Mag_2, 8, 0));

            SMGTitleTypeIndex Ak_Mag_None = new SMGTitleTypeIndex(GunTitles.Ak_74, ModifierTypes.Mag, ModifierIndex.None);
            SMGTitleTypeIndex Ak_Mag_1 = new SMGTitleTypeIndex(GunTitles.Ak_74, ModifierTypes.Mag, ModifierIndex._1);
            SMGTitleTypeIndex Ak_Mag_2 = new SMGTitleTypeIndex(GunTitles.Ak_74, ModifierTypes.Mag, ModifierIndex._2);
            SMGTitleTypeIndex Ak_Mag_3 = new SMGTitleTypeIndex(GunTitles.Ak_74, ModifierTypes.Mag, ModifierIndex._3);
            modifiersCharacteristics.Add(Ak_Mag_None, new SMGModifierItem(Ak_Mag_None, 0, 0));
            modifiersCharacteristics.Add(Ak_Mag_1, new SMGModifierItem(Ak_Mag_1, 24, 0));
            modifiersCharacteristics.Add(Ak_Mag_2, new SMGModifierItem(Ak_Mag_2, 30, 0));
            modifiersCharacteristics.Add(Ak_Mag_3, new SMGModifierItem(Ak_Mag_3, 33, 0));

            modsDescrtiptions = new Dictionary<SMGTitleTypeIndex, (string title, string description, Sprite sprite)>
            {
            { Makarov_Mag_None, ("","", null) },
            { Makarov_Mag_1, ("Магазин 'Антихрист1' ", "Этот магазин был создан в индии под руководством короля Ильнара Абудала Афанасьевича в 1999г.", modifiersCharacteristics[Makarov_Mag_1].sprite) },
            { Makarov_Mag_2, ("Магазин 'Антихрист2' ", "Этот магазин был создан в индии под руководством короля Ильнара Абудала Афанасьевича в 19992г.", modifiersCharacteristics[Makarov_Mag_2].sprite) },

            { TT_Mag_None, ("","", null) },
            { TT_Mag_1, ("Магазин 'Антихрист1' ", "Этот магазин был создан в индии под руководством короля Ильнара Абудала Афанасьевича в 1999г.", modifiersCharacteristics[TT_Mag_1].sprite) },
            { TT_Mag_2, ("Магазин 'Антихрист2' ", "Этот магазин был создан в индии под руководством короля Ильнара Абудала Афанасьевича в 19992г.", modifiersCharacteristics[TT_Mag_2].sprite) },

            { Ak_Mag_None, ("","", null) },
            { Ak_Mag_1, ("Магазин 'Антихрист1' ", "Этот магазин был создан в индии под руководством короля Ильнара Абудала Афанасьевича в 1999г.", modifiersCharacteristics[Ak_Mag_1].sprite) },
            { Ak_Mag_2, ("Магазин 'Антихрист2' ", "Этот магазин был создан в индии под руководством короля Ильнара Абудала Афанасьевича в 19992г.", modifiersCharacteristics[Ak_Mag_2].sprite) },
            { Ak_Mag_3, ("Магазин 'Антихрист3' ", "Этот магазин был создан в индии под руководством короля Ильнара Абудала Афанасьевича в 19992г.", modifiersCharacteristics[Ak_Mag_3].sprite) }
        };
        }

        internal static GunTitles GetGunIdFromInvId(int id)
        {
            if (id == (int)Inventory.ItemStates.ItemsID.Makarov)
                return GunTitles.Makarov;
            if (id == (int)Inventory.ItemStates.ItemsID.TTPistol)
                return GunTitles.TT_Pistol;
            if (id == (int)Inventory.ItemStates.ItemsID.Ak_74)
                return GunTitles.Ak_74;
            return GunTitles.None;
        }

        internal static (string title, string description, Sprite sprite) GetTitleDescSprite(SMGTitleTypeIndex tti)
        {
            return modsDescrtiptions[tti];
        }
        internal static int GetAmmoCountFromDispenser(int title, int dispenserLevel)
        {
            return modifiersCharacteristics[new SMGTitleTypeIndex((GunTitles)title, ModifierTypes.Mag, (ModifierIndex)dispenserLevel)].ammoCount;
        }
        internal static Sprite GetSprite(SMGTitleTypeIndex modState)
        {
            return modifiersCharacteristics[modState].sprite;
        }

        public struct SMGTitleTypeIndex
        {
            public readonly GunTitles Title;
            public readonly ModifierTypes Type;
            public readonly ModifierIndex Index;
            public SMGTitleTypeIndex(GunTitles gT, ModifierTypes mT, ModifierIndex mI)
            {
                Title = gT;
                Type = mT;
                Index = mI;
            }
        }
        public class SMGModifierItem
        {
            public readonly Sprite sprite;
            public readonly int ammoCount;
            public readonly int noisePower;
            public SMGModifierItem(SMGTitleTypeIndex tti, int aC, int nP)
            {
                sprite = Resources.Load<Sprite>($"SMGModifiers\\{tti.Title}\\{tti.Type}\\{tti.Index}");
                ammoCount = aC;
                noisePower = nP;
            }
        }
    }
}