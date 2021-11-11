using Society.Patterns;

using System.Linq;
using UnityEngine;
namespace Society.Missions
{
    /// <summary>
    /// класс вызывает при контакте событие чекпоинта
    /// </summary>
    public class MissionInteractiveObject : InteractiveObject
    {
        [SerializeField]
        [Range(1, MissionsManager.MaxMissions)] private int missionNumber = 1;
#if UNITY_EDITOR
        [ReadOnlyField] [SerializeField] private string missionTitle = "N/A";
#endif

        [SerializeField, Range(0, 10)] private int task;
#if UNITY_EDITOR
        [ReadOnlyField] [SerializeField] private string taskTitle = "N/A";
#endif
        private Mission mMission;

        private bool hasInteracted;

        protected virtual void Start()
        {
            mMission = FindObjectsOfType<Mission>().First(m => m.GetMissionNumber() == missionNumber);
        }
        public override void Interact() => Report();
        protected void Report()
        {
            //защита от нажатия не по сценарию
            if (!CanInteract())
                return;
            if (hasInteracted)
                return;
            hasInteracted = true;

            mMission.Report();
        }

        internal bool CanInteract() => ((mMission == MissionsManager.Instance.GetActiveMission()) &&
            (mMission.GetCurrentTask() == (task - 1)));

        internal bool CanForceInteract()=> ((mMission == MissionsManager.Instance.GetActiveMission()) &&
            (mMission.GetCurrentTask() == (task - 2)));
        internal void ForceInteract()
        {
            hasInteracted = true;

            mMission.Report();
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            missionTitle = MissionsManager.MissionInfo.GetMissionTitleByIndex(missionNumber);
            taskTitle = MissionsManager.MissionInfo.GetMissionTaskTitleByIndex(missionNumber, task);
        }
#endif        
    }
}