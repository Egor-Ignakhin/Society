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
        protected int missionItems = 0;        
        protected readonly Dictionary<string, Action> OnTaskActions = new Dictionary<string, Action>();

        protected virtual void StartMission()
        {            
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
                MissionsManager.Instance.ReportTask();
                SetTask(++currentTask);
                OnReportTask();
            }
            else
                OnReportTask(false, true);
        }

        public abstract int MissionNumber { get; }

        public void ContinueMission(int skipLength)
        {
            if (!isInitialized)
                StartMission();
            currentTask = skipLength;
            SetTask(currentTask);
        }

        protected void FinishMission()
        {
            MissionsManager.Instance.FinishMission();
            gameObject.SetActive(false);
        }
        protected void SetTask(int number)
        {
            string neededContent = Localization.LocalizationManager.PathToCurrentLanguageContent(Localization.LocalizationManager.Type.Tasks, MissionNumber, number);

            MissionsManager.Instance.GetTaskDrawer().DrawNewTask(neededContent);
        }

        internal int GetCurrentTask() => currentTask;


        protected abstract void OnReportTask(bool isLoad = false, bool isMissiomItem = false);
    }
}