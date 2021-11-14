
using Society.Inventory.Other;
using Society.Localization;
using Society.Patterns;

using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR

#endif
using UnityEngine;
namespace Society.Missions
{   /// <summary>
    /// Главный за все миссии
    /// </summary>    
    public sealed class MissionsManager : Singleton<MissionsManager>
    {
        public static string SavePath => Directory.GetCurrentDirectory() + "\\Saves\\State.json"; // папка с сохранением    
        private State currentState;// состояние миссий        
        public DescriptionDrawer DescriptionDrawer { get; private set; }
        public const int MinMissions = 0;

        public const int MaxMissions = 4;
        private TaskSystem.TaskDrawer taskDrawer;
        private Mission activeMission;
        [SerializeField] private List<Mission> MissionList = new List<Mission>();

        internal TaskSystem.TaskDrawer GetTaskDrawer() => taskDrawer;

        private void Awake()
        {
            currentState = LoadState();
            LocalizationManager.InitDialogsTasks(currentState);
        }

        private void Start()
        {
            taskDrawer = FindObjectOfType<TaskSystem.TaskDrawer>();

            StartOrContinueMission();
        }

        internal void SetDescriptionDrawer(DescriptionDrawer dD) => DescriptionDrawer = dD;

        /// <summary>
        /// загрузка состояния миссий
        /// </summary>
        public static State LoadState()
        {
            State reState;
            try
            {
                string data = File.ReadAllText(SavePath);
                reState = JsonUtility.FromJson<State>(data);
            }
            catch
            {
                reState = new State();
                if (!File.Exists(SavePath))
                    File.Create(SavePath);
            }

            return reState;
        }

        public Mission GetActiveMission() => activeMission;

        /// <summary>
        /// Сохранение миссий
        /// </summary>
        public static void SaveState(State state)
        {
            string data = JsonUtility.ToJson(state, true);
            File.WriteAllText(SavePath, data);
        }
        /// <summary>
        /// Сбрасывает задачи для активной миссии
        /// </summary>
        private void ResetTasks() => currentState.currentTask = 0;

        /// <summary>
        /// Сообщает при завершении задачи
        /// </summary>
        public void ReportTask()
        {
            currentState.currentTask++;
        }
        /// <summary>
        /// Вызывается для начала или продолжения миссии с последней задачи
        /// </summary>
        /// <param name="num"></param>
        private void StartOrContinueMission()
        {
            var all = MissionList;

            if (all.Count == 0)
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
            {
                foundedMission.ContinueMission(currentState.currentTask);

                activeMission = foundedMission;
            }

        }
        private void OnDisable() => SaveState(currentState);

        /// <summary>
        /// Сообщает при завершении миссии
        /// </summary>
        public void FinishMission()
        {
            currentState.currentMission++;
            ResetTasks();

            LocalizationManager.InitDialogsTasks(currentState);

            StartOrContinueMission();
        }
        public State GetState() => currentState;

        [System.Serializable]
        public class State
        {
            public int currentMission = 0;
            public int currentTask = 0;
        }

        [ExecuteAlways]
        public sealed class MissionInfo : MonoBehaviour
        {
#if UNITY_EDITOR

            private static Dictionary<int, LocalizationManager.TaskContent> infoAboutMissions = new Dictionary<int, LocalizationManager.TaskContent>();

            public static void UpdateInfo()
            {
                if (infoAboutMissions != null)
                    infoAboutMissions.Clear();

                infoAboutMissions = new Dictionary<int, LocalizationManager.TaskContent>();

                List<LocalizationManager.TaskContent> tasks = new List<LocalizationManager.TaskContent>();
                for (int i = 0; i <= MaxMissions; i++)
                {
                    string data = File.ReadAllText(LocalizationManager.GetPathToMission(i));
                    tasks.Add(JsonUtility.FromJson<LocalizationManager.TaskContent>(data));
                    infoAboutMissions.Add(i, tasks[tasks.Count - 1]);
                }
                foreach (var tc in FindObjectsOfType<MissionInteractiveObject>())
                    tc.OnValidate();

            }
            public static string GetMissionTitleByIndex(int missionIndex)
            {
                if ((infoAboutMissions == null) || infoAboutMissions.Count == 0)
                    UpdateInfo();

                try
                {
                    return infoAboutMissions[missionIndex].MissionTitle;
                }
                catch
                {
                    Debug.LogError($"Mission, Task. Invalid index = {missionIndex}");
                    return "ErrorTasks";
                }
            }
            public static string GetMissionTaskTitleByIndex(int missionIndex, int taskIndex)
            {
                if ((infoAboutMissions == null) || infoAboutMissions.Count == 0)
                    UpdateInfo();
                try
                {
                    return infoAboutMissions[missionIndex].Tasks[taskIndex];
                }
                catch
                {
                    Debug.LogError($"Mission, Task. Invalid index = {missionIndex}_{taskIndex}");
                    return "ErrorTasks";
                }
            }

            public static int GetMaxTasksByIndex(int missionIndex) => infoAboutMissions[missionIndex].Tasks.Count;
#endif
        }
        /// <summary>
        /// Насильный пропуск задачи
        /// </summary>
        internal void SkipTask()
        {
            activeMission.SkipTask();
        }
    }
}