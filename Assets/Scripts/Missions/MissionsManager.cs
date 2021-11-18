
using Newtonsoft.Json;

using Society.Inventory.Other;
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
        public static string SavePath => Directory.GetCurrentDirectory() + "\\Saves\\PlotState.json"; // папка с сохранением    
        private static PlotState plotState;// состояние миссий        
        public DescriptionDrawer DescriptionDrawer { get; private set; }
        public const int MinMissions = 0;

        public const int MaxMissions = 4;
        private TaskSystem.TaskDrawer taskDrawer;
        private Mission activeMission;
        [SerializeField] private List<Mission> MissionList = new List<Mission>();

        internal TaskSystem.TaskDrawer GetTaskDrawer() => taskDrawer;

        private void Awake() => InitializePlotState();

        private void Start()
        {
            taskDrawer = FindObjectOfType<TaskSystem.TaskDrawer>();

            StartOrContinueMission();
        }

        internal void SetDescriptionDrawer(DescriptionDrawer dD) => DescriptionDrawer = dD;

        /// <summary>
        /// Инициализация состояния миссий
        /// </summary>
        private static void InitializePlotState()
        {
            try
            {
                string data = File.ReadAllText(SavePath);
                plotState = JsonConvert.DeserializeObject<PlotState>(data);
            }
            catch
            {
                plotState = new PlotState();
                if (!File.Exists(SavePath))
                    File.Create(SavePath);
            }
        }
        /// <summary>
        /// Возвращает активную миссию
        /// </summary>
        /// <returns></returns>
        public Mission GetActiveMission() => activeMission;

        /// <summary>
        /// Сохранение миссий
        /// </summary>
        public static void SaveState()
        {
            string data = JsonUtility.ToJson(GetPlotState(), true);
            File.WriteAllText(SavePath, data);
        }

        /// <summary>
        /// Сбрасывает задачи для активной миссии
        /// </summary>
        private void ResetTasks() => plotState.currentTask = 0;

        /// <summary>
        /// Сообщает при завершении задачи
        /// </summary>
        public void ReportTask() => plotState.currentTask++;

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
                if (m.GetMissionNumber() == plotState.currentMission)
                {
                    foundedMission = m;
                    break;
                }
            }
            if (foundedMission)
            {
                activeMission = foundedMission;

                foundedMission.ContinueMission(GetPlotState().currentTask);                
            }

        }

        private void OnDisable() => SaveState();

        /// <summary>
        /// Сообщает при завершении миссии
        /// </summary>
        public void FinishMission()
        {
            plotState.currentMission++;
            ResetTasks();

            StartOrContinueMission();
        }

        /// <summary>
        /// Возвращает состояния сценария
        /// </summary>
        /// <returns></returns>
        public static PlotState GetPlotState()
        {
            if (plotState == null)
                InitializePlotState();
            return plotState;
        }

        /// <summary>
        /// Насильный пропуск задачи
        /// </summary>
        internal void SkipTask() => activeMission.SkipTask();
    }
}