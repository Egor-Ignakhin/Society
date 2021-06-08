using System.Collections.Generic;
using UnityEngine;
using static SMG.ModifierCharacteristics;
namespace SMG
{
    static class ModifiersPrefabsData
    {
        private static readonly Dictionary<SMGTitleTypeIndex, GunModifier> modPrefabs;
        static ModifiersPrefabsData()
        {
            modPrefabs = new Dictionary<SMGTitleTypeIndex, GunModifier>
            {
                {new SMGTitleTypeIndex(GunTitles.TT_Pistol,ModifierTypes.Mag, ModifierIndex._1), Resources.Load<GunModifier>("SMG\\TT_Mag_1")},
                {new SMGTitleTypeIndex(GunTitles.TT_Pistol,ModifierTypes.Aim, ModifierIndex._1), Resources.Load<GunModifier>("SMG\\TT_Aim_1")},
                {new SMGTitleTypeIndex(GunTitles.TT_Pistol,ModifierTypes.Aim, ModifierIndex._2), Resources.Load<GunModifier>("SMG\\TT_Aim_2")}
            };
        }
        public static GunModifier GetPrefabFromTTI(SMGTitleTypeIndex tti)
        {
            return modPrefabs[tti];
        }
    }
}