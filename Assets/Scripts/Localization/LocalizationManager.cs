using System.Collections.Generic;
using System.IO;

using Society.Patterns;

using UnityEngine;
namespace Society.Localization
{
    public static class LocalizationManager
    {
        private static Dictionary<string, string> upKeysDescriptions;// словарь для подсказок (нажатия на клавишу)    
        internal static string GetUpKeyDescription(string mainType, KeyCode inputInteractive) =>
             $"{upKeysDescriptions[mainType]} ({inputInteractive})";

        private static HintsData hintsData;
        private static ItemPropertiesData itemProperties;
        private static NutrientItems nutrientItems;
        private static MedicalItems medicalItems;

        public enum Type { Dialogs, Tasks, Hints }

        /// <summary>
        /// Возвращает путь до файла миссии по ей идентификатору
        /// </summary>
        /// <param name="missionId"></param>
        /// <returns></returns>
        public static string GetPathToMission(int missionId)
        {
            return $"{Directory.GetCurrentDirectory()}\\Localization\\Missions\\Mission_{missionId}.json";
        }
        public static class MainTypes
        {
            public const string Default = "Взаимодествовать";
            public const string Item = "Поднять предмет";
        }
        private static readonly List<TaskContent> taskContents = new List<TaskContent>();
        //   private static firstMDialogsContent dialogContent;

        [System.Serializable]
        public class TaskContent
        {
            public string MissionTitle;
            public List<string> Tasks;// пути к задачам
            public string GetTask(int ch) => Tasks[ch];
            public int GetNumberTasks() => Tasks.Count;
        }

        /*    [System.Serializable]
            public class firstMDialogsContent
            {
                public List<string> Dialogs;// пути к диалогам
                public string GetTask(int ch) => Dialogs[ch];
            }*/

        public static void InitDialogsTasks(Missions.MissionsManager.State tempState)
        {
            // инициализация путей
            #region SetDialogs
            {
                //  string data = File.ReadAllText($"{Directory.GetCurrentDirectory()}\\Localization\\Missions\\MissionDialogs_{tempState.currentMission}.json");
                // dialogContent = JsonUtility.FromJson<firstMDialogsContent>(data);
            }
            #endregion
            #region SetTasks
            {
                for (int i = 0; i <= Missions.MissionsManager.MaxMissions; i++)
                {                    
                    string data = File.ReadAllText(GetPathToMission(i));
                    taskContents.Add(JsonUtility.FromJson<TaskContent>(data));
                }
            }
            #endregion

            #region SetUpLeys
            upKeysDescriptions = new Dictionary<string, string>
        {
            {string.Empty, MainTypes.Default},
            {MainTypes.Item, MainTypes.Item}
        };
            #endregion
        }
        #region InitRegion
        private static void InitHD()
        {
            string hdata = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Localization\\Descriptions.json");
            hintsData = JsonUtility.FromJson<HintsData>(hdata);
            hintsData.hints = new Dictionary<string, string>();
            for (int i = 0; i < hintsData.Types.Count; i++)
            {
                hintsData.hints.Add(hintsData.Types[i].Type, hintsData.Types[i].Desc);
            }
        }
        private static void InitMCW()
        {
            string mcvData = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Localization\\WeightAndMaxCountItems.json");
            itemProperties = JsonUtility.FromJson<ItemPropertiesData>(mcvData);
            itemProperties.WeightAndMaxCountItems = new Dictionary<int, (int maxCount, decimal weight)>();
            for (int i = 0; i < itemProperties.Properties.Count; i++)
            {
                System.Enum.TryParse($"{itemProperties.Properties[i].Type}", out Society.Inventory.ItemStates.ItemsID myStatus);
                itemProperties.WeightAndMaxCountItems.Add((int)myStatus, (itemProperties.Properties[i].MaxCount, (decimal)itemProperties.Properties[i].Weight));
            }
        }
        private static void InitNTRS()
        {
            string data = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Localization\\NutritiousItems.json");
            nutrientItems = JsonUtility.FromJson<NutrientItems>(data);
            nutrientItems.FoodWaterItems = new Dictionary<int, (int food, int water)>();
            for (int i = 0; i < nutrientItems.MNutritious.Count; i++)
            {
                System.Enum.TryParse($"{nutrientItems.MNutritious[i].Type}", out Society.Inventory.ItemStates.ItemsID myStatus);
                nutrientItems.FoodWaterItems.Add((int)myStatus, (nutrientItems.MNutritious[i].Food, nutrientItems.MNutritious[i].Water));
            }
        }
        private static void InitMed()
        {
            string data = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Localization\\MedicalItems.json");
            medicalItems = JsonUtility.FromJson<MedicalItems>(data);
            medicalItems.medItems = new Dictionary<int, (int health, int radiation)>();
            for (int i = 0; i < medicalItems.Items.Count; i++)
            {
                System.Enum.TryParse($"{medicalItems.Items[i].Type}", out Society.Inventory.ItemStates.ItemsID myStatus);
                medicalItems.medItems.Add((int)myStatus, (medicalItems.Items[i].Health, medicalItems.Items[i].Radiation));
            }
        }
        #endregion
        public class HintsData : SerialisableInventoryList
        {
            public List<Hint> Types;
            public Dictionary<string, string> hints;
            public string GetHint(string t)
            {
                return hints[t];
            }

            [System.Serializable]
            public class Hint : SerialisableInventoryItem
            {
                public string Desc;
            }

        }
        public class ItemPropertiesData : SerialisableInventoryList
        {
            public List<ItemProperties> Properties;
            public Dictionary<int, (int maxCount, decimal weight)> WeightAndMaxCountItems;
            public int GetMaxCount(int id) => WeightAndMaxCountItems[id].maxCount;
            public decimal GetWight(int id) => WeightAndMaxCountItems[id].weight;

            [System.Serializable]
            public class ItemProperties : SerialisableInventoryItem
            {
                public int MaxCount;
                public float Weight;
            }
        }
        public class NutrientItems : SerialisableInventoryList
        {
            public List<Nutritious> MNutritious;
            public Dictionary<int, (int food, int water)> FoodWaterItems;
            [System.Serializable]
            public class Nutritious : SerialisableInventoryItem
            {
                public int Food;
                public int Water;
            }

            internal (int food, int water) GetProperties(int id)
            {
                return FoodWaterItems[id];
            }
        }
        public class MedicalItems : SerialisableInventoryList
        {
            public List<Medicals> Items;
            public Dictionary<int, (int health, int radiation)> medItems;
            [System.Serializable]
            public class Medicals : SerialisableInventoryItem
            {
                public int Health;
                public int Radiation;
            }

            internal (int health, int radiation) GetProperties(int id)
            {
                return medItems[id];
            }
        }
        public static string PathToCurrentLanguageContent(Type type, int missionNumber, int checkpoint)// возвращает путь до нужного содержимого
        {
            switch (type)
            {
                //   case Type.Dialogs:
                //     return GetDialog(missionNumber, checkpoint);
                case Type.Tasks:
                    return GetTask(missionNumber, checkpoint);
                default:
                    return null;
            }
        }
        //private static string GetDialog(int missionNumber, int checkpoint) => dialogContent.GetTask(checkpoint);

        private static string GetTask(int missionNumber, int checkpoint) => taskContents[missionNumber].GetTask(checkpoint);
        public static int GetNumberOfMissionTasks(int missionNumber) => taskContents[missionNumber].GetNumberTasks();
        public static int GetNumberOfMissionTasks()
        {
            int temp = 0;
            for (int i = 0; i <= Missions.MissionsManager.MaxMissions; i++)
            {
                temp += taskContents[i].GetNumberTasks();
            }
            return temp;
        }

        internal static int GetNumberOfCompletedMissionTasks(int currentMissionNumber, int currentTaskNumber)
        {
            int temp = 0;
            for (int i = 0; i <= Missions.MissionsManager.MaxMissions; i++)
            {
                //Если тек. миссия моложе итерационной
                if (currentMissionNumber > i)
                {
                    temp += taskContents[i].GetNumberTasks();// прибавить к текущим задачам все итерационные
                }
                else if (currentMissionNumber == i)
                {
                    temp += Mathf.Clamp(currentTaskNumber, 0, int.MaxValue);
                }
            }
            return temp;
        }

        public static string GetHint(InteractiveObject interactiveObject)
        {
            if (hintsData == null)
                InitHD();
            return interactiveObject.Type != null ? hintsData.GetHint(interactiveObject.Type) : null;
        }
        public static string GetHint(int id)
        {
            if (hintsData == null)
                InitHD();
            return id != 0 ? hintsData.GetHint(((Society.Inventory.ItemStates.ItemsID)id).ToString()) : null;
        }
        public static int GetMaxCountItem(int id)
        {
            if (itemProperties == null)
                InitMCW();
            return itemProperties.GetMaxCount(id);
        }
        internal static decimal GetWeightItem(int id)
        {
            if (itemProperties == null)
                InitMCW();
            return itemProperties.GetWight(id);
        }
        internal static (int food, int water) GetNutrition(int id)
        {
            if (nutrientItems == null)
                InitNTRS();
            return nutrientItems.GetProperties(id);
        }

        internal static (float health, float radiation) GetMedicalProperties(int id)
        {
            if (nutrientItems == null)
                InitMed();
            return medicalItems.GetProperties(id);
        }
    }
}