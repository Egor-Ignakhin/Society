using UnityEngine;
namespace Missions
{
    sealed class MissionItem : InteractiveObject
    {
        [SerializeField] private string startedType;
        private void Start() => SetType(startedType);

        public override void Interact()
        {
            MissionsManager.GetActiveMission().OnAddMissionItem();
            gameObject.SetActive(false);
        }
    }
}