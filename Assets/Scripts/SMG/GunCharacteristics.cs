using System.Collections.Generic;
using System.Threading.Tasks;
using static Inventory.ItemStates;

namespace SMG
{
    static class GunCharacteristics
    {
        private static readonly Dictionary<ItemsID, (string Title, int damage, int maxFlyD, int OptFlyD, int Caliber)> guns;
        private static GunModifierDescription modDescriptions;
        static GunCharacteristics()
        {//реализовать реальное описание
            guns = new Dictionary<ItemsID, (string title, int damage, int maxFlyD, int OptFlyD, int Caliber)>
            {
                {ItemsID.Makarov,("Пистолет Макарова (ПМ)",10, 10, 10,10)},
                {ItemsID.TTPistol,("Тульский Токарев обр. 1933г.",20, 20, 20,20)},
                {ItemsID.Ak_74,("Автомат Калашникова 5.5x39 (АК-74M)", 30, 30, 30,30)}
            };

            LoadGMD();
        }
        private static void LoadGMD()
        {
            string dataJson = System.IO.File.ReadAllText("Localization\\SMGDescriptions.json");
            modDescriptions = UnityEngine.JsonUtility.FromJson<GunModifierDescription>(dataJson);
            for (int i = 0; i < modDescriptions.Modifiers.Count; i++)
            {
                modDescriptions.titleAndBC.Add(modDescriptions.Modifiers[i].TTI, (modDescriptions.Modifiers[i].BulletsCount, modDescriptions.Modifiers[i].Title, modDescriptions.Modifiers[i].Description));
            }
        }
        public static (string title, int damage, int maxFlyD, int OptFlyD, int Caliber) GetGunCharacteristics(int id) => guns[(ItemsID)id];
        public static int GetBulletsCountFromTTI(ModifierCharacteristics.SMGTitleTypeIndex tti)
        {
            string modTitle = $"{tti.Title}_{tti.Type}{tti.Index}";
            if (modDescriptions.titleAndBC.ContainsKey(modTitle))
                return modDescriptions.titleAndBC[modTitle].bc;
            else return 0;
        }
        public static string GetNormalTitleFromTTI(ModifierCharacteristics.SMGTitleTypeIndex tti)
        {
            string modTitle = $"{tti.Title}_{tti.Type}{tti.Index}";
            if (modDescriptions.titleAndBC.ContainsKey(modTitle))
                return modDescriptions.titleAndBC[modTitle].title;
            else return string.Empty;
        }
        public static string GetNormalDescriptionFromTTI(ModifierCharacteristics.SMGTitleTypeIndex tti)
        {
            string modTitle = $"{tti.Title}_{tti.Type}{tti.Index}";
            if (modDescriptions.titleAndBC.ContainsKey(modTitle))
                return modDescriptions.titleAndBC[modTitle].desc;
            else return string.Empty;
        }
    }
    [System.Serializable]
    public class GunModifierDescription
    {
        public List<Modifier> Modifiers;        
        public Dictionary<string, (int bc, string title, string desc)> titleAndBC { get; set; } = new Dictionary<string, (int bc, string title, string desc)>();

        [System.Serializable]
        public class Modifier
        {
            public string TTI;
            public int BulletsCount;
            public string Title;
            public string Description;
        }
    }
}