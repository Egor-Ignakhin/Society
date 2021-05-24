using System.Collections.Generic;
using UnityEngine;

namespace SMG
{
    public static class SMGModifierCharacteristics
    {
        public enum GunTitles { None, Makarov, TT_Pistol, Ak_74 }
        public enum ModifierTypes { None, Silencer, Mag }
        public enum ModifierIndex { _1, _2, _3 }

        //доступ по имени оружия, типу мода и индксу мода и возврат всех возможных характеристик
        public static readonly Dictionary<(GunTitles, ModifierTypes, ModifierIndex), SMGModifierItem> modifiersCharacteristics;
        static SMGModifierCharacteristics()
        {            
            modifiersCharacteristics = new Dictionary<(GunTitles, ModifierTypes, ModifierIndex), SMGModifierItem>
            {
                {(GunTitles.TT_Pistol, ModifierTypes.Mag, ModifierIndex._1), new SMGModifierItem(GunTitles.TT_Pistol, ModifierTypes.Mag, ModifierIndex._1, 6, 0) },
                {(GunTitles.TT_Pistol, ModifierTypes.Mag, ModifierIndex._2), new SMGModifierItem(GunTitles.TT_Pistol, ModifierTypes.Mag, ModifierIndex._2, 8, 0) }
            };
        }

        internal static Sprite GetSprite((GunTitles modifiableGun, ModifierTypes type, ModifierIndex index) modState)
        {            
            return modifiersCharacteristics[modState].sprite;
        }

        public class SMGModifierItem
        {
            public readonly Sprite sprite;
            public readonly int ammoCount;
            public readonly int noisePower;
            public SMGModifierItem(GunTitles title, ModifierTypes type, ModifierIndex index, int aC, int nP)
            {
                sprite = Resources.Load<Sprite>($"SMGModifiers\\{title}\\{type}\\{index}");           
                ammoCount = aC;
                noisePower = nP;
            }
        }
    }
}