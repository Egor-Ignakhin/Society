using System.IO;
using UnityEngine;
namespace Missions
{/// <summary>
/// Главный за все миссии
/// </summary>
    public sealed class MissionsManager : MonoBehaviour
    {
        public static string savePath = Directory.GetCurrentDirectory() + "\\Saves\\State.json";// папка с сохранением    
        private State currentState;// состояние миссий        
        private void Awake()
        {
            Localization.Init();
        }
        private void OnEnable()
        {
            LoadState();
            StartOrContinueMission(currentState.currentMission);
        }

        /// <summary>
        /// загрузка состояния миссий
        /// </summary>
        private void LoadState()
        {
            try
            {
                string data = File.ReadAllText(savePath);
                currentState = JsonUtility.FromJson<State>(data);
            }
            catch
            {
                currentState = new State();
                if (!File.Exists(savePath))
                    File.Create(savePath);
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
            File.WriteAllText(savePath, data);
        }
        /// <summary>
        /// Сбрасывает задачи для активной миссии
        /// </summary>
        public void ResetTasks() => currentState.currentTask = 0;
        /// <summary>
        /// Сообщает при завершении миссии
        /// </summary>
        public void ReportMission()
        {
            currentState.currentMission++;
            ResetTasks();
        }
        /// <summary>
        /// Сообщает при завершении задачи
        /// </summary>
        public void ReportTask() => currentState.currentTask++;
        /// <summary>
        /// Вызывается для начала или продолжения миссии с последней задачи
        /// </summary>
        /// <param name="num"></param>
        public void StartOrContinueMission(int num)
        {
            switch (num)
            {
                case 0:
                    FindObjectOfType<PrologMission>().ContinueMission(currentState.currentTask);
                    break;
            }
        }
        private void OnDisable() => SaveState();
        public void FinishMission() => ReportMission();

        [System.Serializable]
        public class State
        {
            public int currentMission = 0;
            public int currentTask = 0;
        }
    }
}