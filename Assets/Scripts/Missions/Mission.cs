using System;
using System.Collections.Generic;
using System.Linq;

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

        public abstract int GetMissionNumber();

        public void ContinueMission(int skipLength)
        {
            if (!isInitialized)
                StartMission();
            currentTask = skipLength;
            SetTask(currentTask);
        }

        public virtual void FinishMission()
        {
            MissionsManager.Instance.FinishMission();
            gameObject.SetActive(false);
        }
        protected void SetTask(int number)
        {
            string neededContent = Localization.LocalizationManager.PathToCurrentLanguageContent(Localization.LocalizationManager.Type.Tasks, GetMissionNumber(), number);

            MissionsManager.Instance.GetTaskDrawer().DrawNewTask(neededContent);
        }

        internal int GetCurrentTask() => currentTask;


        protected abstract void OnReportTask(bool isLoad = false, bool isMissiomItem = false);

        /// <summary>
        /// Насильный пропуск задачи
        /// </summary>
        internal void SkipTask()
        {
            List<MissionInteractiveObject> mioArr = FindObjectsOfType<MissionInteractiveObject>().ToList();

            //Удаление из массива тех трекеров, чьи миссии не совпадают с активной
            for (int i = 0; i < mioArr.Count; i++)
            {
                if (!mioArr[i].GetMission().Equals(MissionsManager.Instance.GetActiveMission()))
                {
                    mioArr.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < mioArr.Count; i++)
            {
                if (mioArr[i].GetTask() < (currentTask))
                {
                    mioArr.RemoveAt(i);
                    i--;
                }
            }

            var sortedMioArr = mioArr.OrderBy(m => m.GetTask());// Сортировка по возрастанию задания

            sortedMioArr.First().ForceInteract();
        }
    }
}