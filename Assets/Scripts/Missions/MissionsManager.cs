
using Society.Inventory.Other;
using Society.Localization;

using System.IO;

#if UNITY_EDITOR
using UnityEditor;

#endif
using UnityEngine;
namespace Society.Missions
{   /// <summary>
    /// Главный за все миссии
    /// </summary>    
    public sealed class MissionsManager : MonoBehaviour
    {
        public static string SavePath => Directory.GetCurrentDirectory() + "\\Saves\\State.json"; // папка с сохранением    
        private State currentState;// состояние миссий        
        public DescriptionDrawer DescriptionDrawer { get; private set; }
        public const int MaxMissions = 2;
        private void Awake()
        {
            LoadState();
            LocalizationManager.Init(currentState);
        }

        private void Start()
        {
            StartOrContinueMission();
        }

        internal void SetDescriptionDrawer(DescriptionDrawer dD) => DescriptionDrawer = dD;

        /// <summary>
        /// загрузка состояния миссий
        /// </summary>
        private void LoadState()
        {
            try
            {
                string data = File.ReadAllText(SavePath);
                currentState = JsonUtility.FromJson<State>(data);
            }
            catch
            {
                currentState = new State();
                if (!File.Exists(SavePath))
                    File.Create(SavePath);
            }
        }
        /// <summary>
        /// возвращает активную миссию
        /// </summary>
        /// <returns></returns>
        public static Mission GetActiveMission() => FindObjectOfType<PrologMission>();
        /// <summary>
        /// Сохранение миссий
        /// </summary>
        private void SaveState()
        {
            string data = JsonUtility.ToJson(currentState, true);
            File.WriteAllText(SavePath, data);
        }
        /// <summary>
        /// Сбрасывает задачи для активной миссии
        /// </summary>
        private void ResetTasks() => currentState.currentTask = 0;

        /// <summary>
        /// Сообщает при завершении задачи
        /// </summary>
        public void ReportTask() => currentState.currentTask++;
        /// <summary>
        /// Вызывается для начала или продолжения миссии с последней задачи
        /// </summary>
        /// <param name="num"></param>
        private void StartOrContinueMission()
        {
            var all = FindObjectsOfType<Mission>();

            if (all.Length == 0)
                return;
            Mission foundedMission = null;
            foreach (var m in all)
            {
                if (m.GetMissionNumber() == currentState.currentMission)
                {
                    foundedMission = m;
                    break;
                }
            }
            if (foundedMission)
                foundedMission.ContinueMission(currentState.currentTask);
        }
        private void OnDisable() => SaveState();

        /// <summary>
        /// Сообщает при завершении миссии
        /// </summary>
        public void FinishMission()
        {
            currentState.currentMission++;
            ResetTasks();
        }

        [System.Serializable]
        public class State
        {
            public int currentMission = 1;
            public int currentTask = 0;
        }

        [ExecuteAlways]
        public sealed class MissionInfo : MonoBehaviour
        {
#if UNITY_EDITOR

            private static System.Collections.Generic.Dictionary<int, LocalizationManager.TaskContent> infoAboutMissions = new System.Collections.Generic.Dictionary<int, LocalizationManager.TaskContent>();

            [MenuItem("Tools/Update Info About Missions")]
            private static void UpdateInfoAboutMissions()
            {
                if (infoAboutMissions != null)
                    infoAboutMissions.Clear();

                infoAboutMissions = new System.Collections.Generic.Dictionary<int, LocalizationManager.TaskContent>();

                System.Collections.Generic.List<LocalizationManager.TaskContent> tasks = new System.Collections.Generic.List<LocalizationManager.TaskContent>();
                for (int i = 1; i <= MaxMissions; i++)
                {
                    string path = $"Localization\\Missions\\MissionTask_{i}.json";
                    string data = File.ReadAllText(path);
                    tasks.Add(JsonUtility.FromJson<LocalizationManager.TaskContent>(data));
                    infoAboutMissions.Add(i, tasks[tasks.Count - 1]);
                }
                foreach (var tc in FindObjectsOfType<MissionInteractiveObject>())
                    tc.OnValidate();

            }
            public static string GetMissionTitleByIndex(int index)
            {
                if ((infoAboutMissions == null) || infoAboutMissions.Count == 0)
                    UpdateInfoAboutMissions();

                return infoAboutMissions[index].MissionTitle;
            }
            public static string GetMissionTaskTitleByIndex(int missionIndex, int taskIndex)
            {
                if ((infoAboutMissions == null) || infoAboutMissions.Count == 0)
                    UpdateInfoAboutMissions();

                return infoAboutMissions[missionIndex].Tasks[taskIndex];
            }
#endif
        }
    }
}