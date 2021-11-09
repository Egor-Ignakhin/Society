using System;
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
            for (int g = 1; g < Enum.GetValues(typeof(GunTitles)).Length; g++)
            {
                for (int t = 1; t < Enum.GetValues(typeof(ModifierTypes)).Length; t++)
                {
                    for (int i = 1; i < Enum.GetValues(typeof(ModifierIndex)).Length; i++)
                    {
                        modPrefabs.Add(new SMGTitleTypeIndex((GunTitles)g, (ModifierTypes)t, (ModifierIndex)i), Resources.Load<GunModifier>($"SMG\\{(GunTitles)g}_{(ModifierTypes)t}{(ModifierIndex)i}"));
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