using Society.Inventory;

using System.Collections.Generic;

using UnityEngine;

namespace Society.SMG
{
    public static class ModifierCharacteristics
    {
        /// <summary>
        /// Названия оружий
        /// </summary>
        public enum GunTitles { None, TT_Pistol, Ak_74 }
        /// <summary>
        /// Типа модификаций
        /// </summary>
        public enum ModifierTypes { None, Silencer, Mag, Aim }
        /// <summary>
        /// Уровни модификаций
        /// </summary>
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
                for (int modifierType = 1; modifierType < 4; modifierType++)
                {
                    for (int modifierIndex = 1; modifierIndex < 3; modifierIndex++)
                    {
                        ttis.Add(new SMGTitleTypeIndex((GunTitles)g, (ModifierTypes)modifierType, (ModifierIndex)modifierIndex));

                        int index = modifiersCharacteristics.Count;
                        modifiersCharacteristics.Add(ttis[index], new SMGModifierItem(ttis[index], GunCharacteristics.GetBulletsCountFromTTI(ttis[index])));
                        modsDescrtiptions.Add(ttis[index], (GunCharacteristics.GetNormalTitleFromTTI(ttis[index]),
                            GunCharacteristics.GetNormalDescriptionFromTTI(ttis[index]), modifiersCharacteristics[ttis[0]].sprite));
                    }
                }
            }
        }

        internal static (string title, string description, Sprite sprite) GetTitleDescSprite(SMGTitleTypeIndex tti) => modsDescrtiptions[tti];
        internal static int GetAmmoCountFromDispenser(int title, int dispenserLevel)
        {
            var key = new SMGTitleTypeIndex((GunTitles)title, ModifierTypes.Mag, (ModifierIndex)dispenserLevel);
            if (modifiersCharacteristics.ContainsKey(key))
                return modifiersCharacteristics[key].ammoCount;
            else return 0;
        }
        internal static Sprite GetSprite(SMGTitleTypeIndex modState)
        {
            if (modifiersCharacteristics.ContainsKey(modState))
                return modifiersCharacteristics[modState].sprite;
            else return null;
        }

        /// <summary>
        /// контейнер сод. информацию о <see cref="GunTitles"/>, <see cref="ModifierTypes"/> и <see cref="ModifierIndex"/>
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