using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

using UnityEngine;
namespace Society.Localization
{
    public static class LocalizationManager
    {
        /// <summary>
        /// Подсказки о взаимодействующих клавишах
        /// </summary>
        private static Dictionary<string, string> upKeysDescriptions;

        /// <summary>
        /// Подсказки по наведению на предмет
        /// </summary>
        private static HintsData hintsData;

        /// <summary>
        /// Свойства предметов
        /// </summary>
        private static ItemPropertiesData itemProperties;

        /// <summary>
        /// Питательные предметы
        /// </summary>
        private static NutrientItems nutrientItems;

        /// <summary>
        /// Медецинские предметы
        /// </summary>
        private static MedicalItems medicalItems;

        private static readonly List<Mission> missions = new List<Mission>();
        static LocalizationManager()
        {
            InitializeMissions();
            InitializeUpKeysDescriptions();
            InitializeHints();
            InitializeWeightMaxCountItems();
            InitializeNutritiousItems();
            InitializeMedicalItems();
        }

        #region Initialization

        /// <summary>
        /// Инициализация миссий
        /// </summary>
        public static void InitializeMissions()
        {
            for (int i = 0; i <= Missions.MissionsManager.MaxMissions; i++)
            {
                string data = File.ReadAllText(GetPathToMission(i));
                missions.Add(JsonConvert.DeserializeObject<Mission>(data));
            }
        }

        /// <summary>
        /// Инициализация подсказок о клавишах
        /// </summary>
        private static void InitializeUpKeysDescriptions()
        {
            upKeysDescriptions = new Dictionary<string, string> {
            {string.Empty, MainTypes.Default},
            {MainTypes.Item, MainTypes.Item}
            };
        }

        /// <summary>
        /// Инициализация подсказок
        /// </summary>
        private static void InitializeHints()
        {
            string hdata = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Localization\\Descriptions.json");
            hintsData = JsonConvert.DeserializeObject<HintsData>(hdata);
            hintsData.hints = new Dictionary<string, string>();
            for (int i = 0; i < hintsData.Types.Count; i++)
            {
                hintsData.hints.Add(hintsData.Types[i].Type, hintsData.Types[i].Desc);
            }
        }

        /// <summary>
        /// Инициализация массы и макс. кол-ва предметов
        /// </summary>
        private static void InitializeWeightMaxCountItems()
        {
            string mcvData = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Localization\\WeightAndMaxCountItems.json");
            itemProperties = JsonConvert.DeserializeObject<ItemPropertiesData>(mcvData);
            itemProperties.WeightAndMaxCountItems = new Dictionary<int, (int maxCount, decimal weight)>();
            for (int i = 0; i < itemProperties.Properties.Count; i++)
            {
                System.Enum.TryParse($"{itemProperties.Properties[i].Type}", out Inventory.ItemStates.ItemsID myStatus);
                itemProperties.WeightAndMaxCountItems.Add((int)myStatus, (itemProperties.Properties[i].MaxCount, (decimal)itemProperties.Properties[i].Weight));
            }
        }

        /// <summary>
        /// Инициализация питательных свойства предметов
        /// </summary>
        private static void InitializeNutritiousItems()
        {
            string data = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Localization\\NutritiousItems.json");
            nutrientItems = JsonConvert.DeserializeObject<NutrientItems>(data);
            nutrientItems.FoodWaterItems = new Dictionary<int, (int food, int water)>();
            for (int i = 0; i < nutrientItems.MNutritious.Count; i++)
            {
                System.Enum.TryParse($"{nutrientItems.MNutritious[i].Type}", out Inventory.ItemStates.ItemsID myStatus);
                nutrientItems.FoodWaterItems.Add((int)myStatus, (nutrientItems.MNutritious[i].Food, nutrientItems.MNutritious[i].Water));
            }
        }

        /// <summary>
        /// Инициализация целебных свойства предметов
        /// </summary>
        private static void InitializeMedicalItems()
        {
            string data = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Localization\\MedicalItems.json");
            medicalItems = JsonConvert.DeserializeObject<MedicalItems>(data);
            medicalItems.medItems = new Dictionary<int, (int health, int radiation)>();
            for (int i = 0; i < medicalItems.Items.Count; i++)
            {
                System.Enum.TryParse($"{medicalItems.Items[i].Type}", out Inventory.ItemStates.ItemsID myStatus);
                medicalItems.medItems.Add((int)myStatus, (medicalItems.Items[i].Health, medicalItems.Items[i].Radiation));
            }
        }

        #endregion                      

        /// <summary>
        /// Возвращает путь до файла миссии
        /// </summary>
        /// <param name="missionId"></param>
        /// <returns></returns>
        public static string GetPathToMission(int missionId) =>
            $"{Directory.GetCurrentDirectory()}\\Localization\\Missions\\Mission_{missionId}.json";

        /// <summary>
        /// Возвращает задачу по ключу
        /// </summary>
        /// <param name="missionNumber"></param>
        /// <param name="checkpoint"></param>
        /// <returns></returns>
        public static string GetTask(int missionNumber, int checkpoint) => missions[missionNumber].GetTask(checkpoint);

        /// <summary>
        /// Возвращает количество задач в миссии
        /// </summary>
        /// <param name="missionNumber"></param>
        /// <returns></returns>
        public static int GetNumberOfMissionTasks(int missionNumber) => missions[missionNumber].GetNumberTasks();

        /// <summary>
        /// Возвращает число сумму всех задач всех миссий
        /// </summary>
        /// <returns></returns>
        public static int GetSumOfAllTasksOfAllMissions()
        {
            int temp = 0;
            for (int i = 0; i <= Missions.MissionsManager.MaxMissions; i++)
                temp += missions[i].GetNumberTasks();

            return temp;
        }

        /// <summary>
        /// Возвращает количество пройденных задач
        /// </summary>
        /// <param name="currentMissionNumber"></param>
        /// <param name="currentTaskNumber"></param>
        /// <returns></returns>
        internal static int GetNumberOfCompletedMissionTasks(int currentMissionNumber, int currentTaskNumber)
        {
            int temp = 0;
            for (int i = 0; i <= Missions.MissionsManager.MaxMissions; i++)
            {
                //Если тек. миссия моложе итерационной
                if (currentMissionNumber > i)
                {
                    temp += missions[i].GetNumberTasks();// прибавить к текущим задачам все итерационные
                }
                else if (currentMissionNumber == i)
                {
                    temp += Mathf.Clamp(currentTaskNumber, 0, int.MaxValue);
                }
            }
            return temp;
        }

        /// <summary>
        /// Возвращает подсказку об объекте
        /// </summary>
        /// <param name="interactiveObject"></param>
        /// <returns></returns>
        public static string GetHint(string type) => type != null ? hintsData.GetHint(type) : null;

        /// <summary>
        /// Возвращает макс. количество предметов в стаке
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int GetMaxCountItem(int id) => itemProperties.GetMaxCount(id);

        /// <summary>
        /// Возвращает вес предмета
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static decimal GetWeightItem(int id) => itemProperties.GetWight(id);

        /// <summary>
        /// Возвращает питательность предмета
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static (int food, int water) GetNutrition(int id) => nutrientItems.GetProperties(id);

        /// <summary>
        /// Возвращает целебные свойства предмета
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static (float health, float radiation) GetSalubrity(int id) => medicalItems.GetSalubrity(id);

        /// <summary>
        /// Возвращает подсказку по наведению на объект
        /// </summary>
        /// <param name="mainType"></param>
        /// <param name="inputInteractive"></param>
        /// <returns></returns>
        internal static string GetUpKeyDescription(string mainType, KeyCode inputInteractive) =>
             $"{upKeysDescriptions[mainType]} ({inputInteractive})";

#if UNITY_EDITOR
        public static string GetMissionTitle(int missionId)
        {
            try
            {
                return missions[missionId].Title;
            }
            catch
            {
                Debug.LogError($"Failed to get mission title. Invalid mission id = {missionId}");
                return "Error";
            }
        }

        public static string GetTaskTitle(int missionId, int taskId)
        {
            try
            {
                return missions[missionId].GetTask(taskId);
            }
            catch
            {
                Debug.LogError($"Failed to get task title. " +
                    $"Invalid mission id = {missionId}, Invalid task id = {taskId}");
                return "Error";
            }            
        }
#endif
    }
}