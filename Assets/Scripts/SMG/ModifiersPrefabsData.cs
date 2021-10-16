using System.Collections.Generic;

using UnityEngine;

using static Society.SMG.ModifierCharacteristics;
namespace Society.SMG
{
    internal static class ModifiersPrefabsData
    {
        private static readonly Dictionary<SMGTitleTypeIndex, GunModifier> modPrefabs = new Dictionary<SMGTitleTypeIndex, GunModifier>();
        static ModifiersPrefabsData()
        {
            for (int g = 1; g < 4; g++)
            {
                for (int t = 1; t < 4; t++)
                {
                    for (int i = 1; i < 3; i++)
                    {
                        modPrefabs.Add(new SMGTitleTypeIndex((GunTitles)g, (ModifierTypes)t, (ModifierIndex)i), Resources.Load<GunModifier>($"Society.SMG\\{(GunTitles)g}_{(ModifierTypes)t}{(ModifierIndex)i}"));
                    }
                }
            }
        }
        public static GunModifier GetPrefabFromTTI(SMGTitleTypeIndex tti)
        {
            return modPrefabs[tti];
        }
    }
}