using System;
using System.Collections.Generic;

using static Society.Inventory.ItemStates;

namespace Society.SMG
{
    static class GunCharacteristics
    {
        private static readonly Dictionary<ItemsID, GunDescription.Gun> guns;
        private static GunModifierDescription modDescriptions;
        private static GunDescription gunDescription;
        static GunCharacteristics()
        {
            LoadGMD();
            LoadGuns();
            guns = new Dictionary<ItemsID, GunDescription.Gun>
            {
                {ItemsID.TTPistol, gunDescription.Data[GunsID.TTPistol]},
                {ItemsID.Ak_74, gunDescription.Data[GunsID.Ak_74]}
            };
        }

        private static void LoadGMD()
        {
            string dataJson = System.IO.File.ReadAllText("Localization\\SMGDescriptions.json");
            modDescriptions = UnityEngine.JsonUtility.FromJson<GunModifierDescription>(dataJson);
            for (int i = 0; i < modDescriptions.Modifiers.Count; i++)
            {
                modDescriptions.Data.Add(modDescriptions.Modifiers[i].TTI, (modDescriptions.Modifiers[i].BulletsCount, modDescriptions.Modifiers[i].Title, modDescriptions.Modifiers[i].Description));
            }
        }

        private static void LoadGuns()
        {
            string dataJson = System.IO.File.ReadAllText("Localization\\GunsDescriptions.json");
            gunDescription = UnityEngine.JsonUtility.FromJson<GunDescription>(dataJson);
            for (int i = 0; i < gunDescription.Guns.Count; i++)
            {
                gunDescription.Data.Add((GunsID)i, gunDescription.Guns[i]);
            }
        }

        internal static string GetGunDescriptionFromTitle(int title) => guns[(ItemsID)title].Description;
        internal static object GetDamage(int id) => guns[(ItemsID)id].Damage;
        public static string GetGunTitle(int id) => guns[(ItemsID)id].Title;
        public static string GetCaliberFromTitle(int id) => guns[(ItemsID)id].Caliber;
        public static string GetOptimalDistanceFromTitle(int id) => guns[(ItemsID)id].OptimalDistance;
        public static string GetMaximumDistanceFromTitle(int id) => guns[(ItemsID)id].MaximumDistance;
        public static int GetBulletsCountFromTTI(ModifierCharacteristics.SMGTitleTypeIndex tti)
        {
            string modTitle = $"{tti.Title}_{tti.Type}{tti.Index}";
            if (modDescriptions.Data.ContainsKey(modTitle))
                return modDescriptions.Data[modTitle].bc;
            else return 0;
        }
        public static string GetNormalTitleFromTTI(ModifierCharacteristics.SMGTitleTypeIndex tti)
        {
            string modTitle = $"{tti.Title}_{tti.Type}{tti.Index}";
            if (modDescriptions.Data.ContainsKey(modTitle))
                return modDescriptions.Data[modTitle].title;
            else return string.Empty;
        }
        public static string GetNormalDescriptionFromTTI(ModifierCharacteristics.SMGTitleTypeIndex tti)
        {
            string modTitle = $"{tti.Title}_{tti.Type}{tti.Index}";
            if (modDescriptions.Data.ContainsKey(modTitle))
                return modDescriptions.Data[modTitle].desc;
            else return string.Empty;
        }

        [Serializable]
        public class GunModifierDescription
        {
            public List<Modifier> Modifiers;
            public Dictionary<string, (int bc, string title, string desc)> Data { get; } = new Dictionary<string, (int bc, string title, string desc)>();

            [Serializable]
            public class Modifier
            {
                public string TTI;
                public int BulletsCount;
                public string Title;
                public string Description;
            }
        }
        [Serializable]
        public class GunDescription
        {
            public List<Gun> Guns;
            public Dictionary<GunsID, Gun> Data { get; } = new Dictionary<GunsID, Gun>();

            [Serializable]
            public class Gun
            {
                public string Title;
                public string Caliber;
                public string OptimalDistance;
                public string MaximumDistance;
                public string Damage;
                public string Description;
            }
        }

        internal static int GetMaxMagFromID(int v)
        {
            if (v == (int)ItemsID.TTPistol)
            {
                return 2;
            }
            return 0;
        }

        internal static int GetMaxAimFromID(int v)
        {
            if (v == (int)ItemsID.TTPistol)
            {
                return 2;
            }
            return 0;
        }

        internal static int GetMaxSilencerFromID(int v)
        {
            if (v == (int)ItemsID.TTPistol)
            {
                return 2;
            }
            return 0;
        }
    }
}