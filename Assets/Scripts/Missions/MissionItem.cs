using System.Linq;

using Society.Localization;
using Society.Patterns;

using UnityEngine;
namespace Society.Missions
{
    internal sealed class MissionItem : InteractiveObject
    {
        /// <summary>
        /// Номер миссии трекера
        /// </summary>
        [SerializeField]
        [Range(MissionsManager.MinMissions, MissionsManager.MaxMissions)] private int missionNumber = 0;
#if UNITY_EDITOR
        /// <summary>
        /// Вспомогательное поле для разработчика - отображает название миссии
        /// </summary>
        [ReadOnlyField] [SerializeField] private string missionTitle = "N/A";
#endif
        /// <summary>
        /// Номер задачи трекера
        /// </summary>
        [SerializeField, Range(0, 10)] private int task;
#if UNITY_EDITOR
        /// <summary>
        /// Вспомогательное поле для разработчика - отображает название задачи
        /// </summary>
        [ReadOnlyField] [SerializeField] private string taskTitle = "N/A";
#endif
        [SerializeField] private string startedType;

        /// <summary>
        /// Миссия трекера
        /// </summary>
        private Mission mMission;

        /// <summary>
        /// Трекер сработал раннее?
        /// </summary>
        private bool hasInteracted;

        /// <summary>
        /// Метка задачи. Используется по нужде.
        /// </summary>
        [SerializeField] private TaskLabel taskLabel;

        protected override void Awake()
        {
            //Определение нужной мисии из всех доступных на сцене            
            mMission = FindObjectsOfType<Mission>().First(m => m.GetMissionNumber() == missionNumber);

            mMission.ChangeTaskEvent += OnChangeTask;

            base.Awake();
        }
        private void Start() => SetType(startedType);

        /// <summary>
        /// Трекер может сработать?
        /// </summary>
        /// <returns></returns>
        internal bool CanInteract()
        {
            return (mMission == MissionsManager.Instance.GetActiveMission()) && //Миссия трекера это активная миссия?
                    (task == mMission.GetCurrentTask());// Задача трекера это активная задача?
        }

        public override void Interact()
        {
            if (!CanInteract())
                return;
            if (hasInteracted)
                return;
            hasInteracted = true;

            mMission.AddMissionItem(this);
            gameObject.SetActive(false);

            if (taskLabel)
            {
                taskLabel.Deactivate();
            }
        }
        private void OnChangeTask(int task)
        {
            if (taskLabel)
            {
                if (CanInteract())
                    taskLabel.Activate();
                else
                    taskLabel.Deactivate();
            }
        }

        private void OnDisable()
        {
            if (mMission)
                mMission.ChangeTaskEvent -= OnChangeTask;
        }


#if UNITY_EDITOR
        public void OnValidate()
        {
            missionTitle = LocalizationManager.GetMissionTitle(missionNumber);
            taskTitle = LocalizationManager.GetTaskTitle(missionNumber, task);
        }
#endif   
    }
}