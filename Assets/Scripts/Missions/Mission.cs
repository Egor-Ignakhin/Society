using Society.Missions.TaskSystem;

using System;
using System.Collections.Generic;

using UnityEngine;
namespace Society.Missions
{
    /// <summary>
    /// абстрактный класс миссии, содержит общие свойства
    /// </summary>
    public abstract class Mission : MonoBehaviour
    {
        private bool isInitialized = false;
        protected int currentTask = 0;
        protected MissionsManager missionsManager;
        protected int missionItems = 0;
        protected TaskDrawer taskDrawer;
        protected readonly Dictionary<string, Action> OnTaskActions = new Dictionary<string, Action>();

        protected virtual void StartMission()
        {
            missionsManager = FindObjectOfType<MissionsManager>();
            taskDrawer = missionsManager.GetTaskDrawer();
            OnReportTask(true);
            isInitialized = true;
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
            if (!isInitialized)
                StartMission();
            currentTask = skipLength;
            SetTask(currentTask);
        }

        protected void FinishMission()
        {
            missionsManager.FinishMission();
            gameObject.SetActive(false);
        }
        protected void SetTask(int number)
        {
            string neededContent = Localization.LocalizationManager.PathToCurrentLanguageContent(Localization.LocalizationManager.Type.Tasks, GetMissionNumber(), number);

            taskDrawer.DrawNewTask(neededContent);
        }

        internal int GetCurrentTask() => currentTask;


        protected abstract void OnReportTask(bool isLoad = false, bool isMissiomItem = false);
    }
}