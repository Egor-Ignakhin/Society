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
        public static readonly Dictionary<SMGTitleTypeIndex, SMGModifierItem> modifiersCharacteristics = new Dictionary<SMGTitleTypeIndex, SMGModifierItem>();
        static SMGModifierCharacteristics()
        {
            SMGTitleTypeIndex TT_Mag_1 = new SMGTitleTypeIndex(GunTitles.TT_Pistol, ModifierTypes.Mag, ModifierIndex._1);
            SMGTitleTypeIndex TT_Mag_2 = new SMGTitleTypeIndex(GunTitles.TT_Pistol, ModifierTypes.Mag, ModifierIndex._2);
            modifiersCharacteristics.Add(TT_Mag_1, new SMGModifierItem(TT_Mag_1, 6, 0));
            modifiersCharacteristics.Add(TT_Mag_2, new SMGModifierItem(TT_Mag_2, 8, 0));
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