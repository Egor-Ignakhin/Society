using UnityEngine;
namespace Missions
{/// <summary>
/// абстрактный класс миссии, содержит общие свойства
/// </summary>
    public abstract class Mission : MonoBehaviour
    {
        protected int currentTask = 0;
        protected MissionsManager missionsManager;
        protected int missionItems = 0;
        protected TaskDrawer taskDrawer;        

        protected virtual void Start()
        {
            missionsManager = FindObjectOfType<MissionsManager>();
            taskDrawer = FindObjectOfType<TaskDrawer>();
            OnReportTask(true);
        }


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
                OnReportTask();
            }
            else                    
                OnReportTask(false, true);            
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
            
            taskDrawer.DrawNewTask(neededContent);
        }

        internal int GetCurrentTask() => currentTask;


        protected abstract void OnReportTask(bool isLoad = false, bool isMissiomItem = false);
    }
}