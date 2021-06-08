using System.Collections.Generic;
using static Inventory.ItemStates;

namespace SMG
{
    static class GunCharacteristics
    {
        private static readonly Dictionary<ItemsID, string> guns;
        private static GunModifierDescription modDescriptions;
        private static GunDescription gunDescription;
        static GunCharacteristics()
        {
            LoadGMD();
            LoadGuns();
            guns = new Dictionary<ItemsID, string>
            {
                {ItemsID.Makarov, gunDescription.Data[GunsID.Makarov].Title},
                {ItemsID.TTPistol, gunDescription.Data[GunsID.TTPistol].Title},
                {ItemsID.Ak_74, gunDescription.Data[GunsID.Ak_74].Title}
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
        public static string GetGunTitle(int id) => guns[(ItemsID)id];
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

        [System.Serializable]
        public class GunModifierDescription
        {
            public List<Modifier> Modifiers;
            public Dictionary<string, (int bc, string title, string desc)> Data { get; } = new Dictionary<string, (int bc, string title, string desc)>();

            [System.Serializable]
            public class Modifier
            {
                public string TTI;
                public int BulletsCount;
                public string Title;
                public string Description;
            }
        }
        [System.Serializable]
        public class GunDescription
        {
            public List<Gun> Guns;
            public Dictionary<GunsID, Gun> Data { get; } = new Dictionary<GunsID, Gun>();

            [System.Serializable]
            public class Gun
            {
                public string Title;
            }
        }
    }
}