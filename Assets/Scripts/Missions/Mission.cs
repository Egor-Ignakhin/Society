using UnityEngine;
namespace Missions
{/// <summary>
/// абстрактный класс миссии, содержит общие свойства
/// </summary>
    public abstract class Mission : MonoBehaviour
    {
        protected int currentTask = 0;
        private MissionsManager missionsManager;
        protected int missionItems = 0;
        protected virtual void Awake() =>
            missionsManager = FindObjectOfType<MissionsManager>();

        private void Start() =>
            OnReportTask(currentTask, true);


        /// <summary>
        /// Вызывается при подборе миссионного предмета (химза, таблетки к примеру)
        /// </summary>
        internal void OnAddMissionItem()
        {
            missionItems++;
            Report(true);
        }

        /// <summary>
        /// событие чекпоинта
        /// </summary>
        public void Report(bool isMissionItem = false)
        {
            if (!isMissionItem)//если сообщение явл. обычнм трекером
            {
                missionsManager.ReportTask();
                SetTask(++currentTask);
                OnReportTask(currentTask);
            }
            else                    
                OnReportTask(currentTask, false, true);            
        }

        public abstract int GetMissionNumber();
        public void ContinueMission(int skipLength)
        {
            currentTask = skipLength;
            SetTask(currentTask);
        }

        public void FinishMission()
        {
            missionsManager.FinishMission();
            gameObject.SetActive(false);
        }
        protected void SetTask(int number)
        {
            string neededContent = Localization.PathToCurrentLanguageContent(Localization.Type.Tasks, GetMissionNumber(), number);

            TaskDrawer.Instance.DrawNewTask(neededContent);
        }

        internal int GetCurrentTask() => currentTask;


        protected abstract void OnReportTask(int currentTask, bool isLoad = false, bool isMissiomItem = false);
    }
}