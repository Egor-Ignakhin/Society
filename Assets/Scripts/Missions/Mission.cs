using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
namespace Society.Missions
{
    /// <summary>
    /// абстрактный класс миссии, содержит общие свойства
    /// </summary>
    internal abstract class Mission : MonoBehaviour
    {
        /// <summary>
        /// Миссия инициализирована?
        /// </summary>
        private bool missionIsInitialized = false;

        /// <summary>
        /// Текущее задание
        /// </summary>
        protected int currentTask = 0;

        /// <summary>
        /// Deprecated! Please use <see cref="taskActions"/>
        /// </summary>
        protected Dictionary<string, Action> OnTaskActions { get; set; } = new Dictionary<string, Action>();

        /// <summary>
        /// Список событий, которые вызываются при смене задания
        /// </summary>
        protected List<Action> taskActions = new List<Action>();

        /// <summary>
        /// Событие смены задания
        /// </summary>
        public event Action<int> ChangeTaskEvent;

        protected abstract Dictionary<MissionItem, bool> MissionItems { get; set; }

        protected virtual void StartMission()
        {
            OnReportTask(true);
            missionIsInitialized = true;
        }


        /// <summary>
        /// Вызывается при подборе миссионного предмета (химза, таблетки к примеру)
        /// </summary>
        internal void AddMissionItem(MissionItem mi)
        {
            MissionItems[mi] = true;
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

                DrawTask(++currentTask);
                OnReportTask();
            }
            else
                OnReportTask(false, true);
        }

        public abstract int GetMissionNumber();

        public void ContinueMission(int skipLength)
        {
            currentTask = skipLength;

            if (!missionIsInitialized)
                StartMission();       
            
            DrawTask(currentTask);
        }

        public virtual void FinishMission()
        {
            MissionsManager.Instance.FinishMission();
            gameObject.SetActive(false);
        }
        protected void DrawTask(int number)
        {
            string neededContent = Localization.LocalizationManager.GetTask(GetMissionNumber(), number);

            //MissionsManager.Instance.TaskDrawer.DrawNewTask(neededContent);
        }

        internal int GetCurrentTask() => currentTask;


        protected virtual void OnReportTask(bool isLoad = false, bool isMissiomItem = false) => ChangeTaskEvent?.Invoke(currentTask);

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