using UnityEngine;

namespace Society.Features.Bunker.EmergencySystem
{
    internal sealed class EmergencyLamp : MonoBehaviour
    {
        private bool isActiveEmergencyState = false;
        [SerializeField] private Animator mAnimator;

        private void OnEnable()
        {
            BunkerEmergencyManager.Instance.ChangeEmergencyTypeEvent += OnChangeEmergencyType;
        }

        private void OnChangeEmergencyType(EmergencyTypes emergencyType)
        {
            if(emergencyType == EmergencyTypes.None)
            {
                mAnimator.enabled = false;
            }
            else
            {
                mAnimator.enabled = true;
            }
        }

        private void OnDisable()
        {
            BunkerEmergencyManager.Instance.ChangeEmergencyTypeEvent -= OnChangeEmergencyType;
        }
    }
}