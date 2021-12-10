
using Society.Features.Bunker.EmergencySystem;

using UnityEngine;

namespace Society.Features.Bunker.Generator
{
    public class GeneratorManager : MonoBehaviour
    {
        [SerializeField] private AudioSource mAudioSource;
        [SerializeField] private AudioClip workClip;
        private void OnEnable()
        {
            OnChangeEmergencyType(EmergencyTypes.None);
            BunkerEmergencyManager.Instance.ChangeEmergencyTypeEvent += OnChangeEmergencyType;
        }

        private void OnChangeEmergencyType(EmergencyTypes emergencyType)
        {
            if (emergencyType == EmergencyTypes.None)
            {
                mAudioSource.clip = workClip;
                mAudioSource.Play();
            }
            else
            {
                mAudioSource.Stop();
            }
        }


        private void OnDisable()
        {
            if (BunkerEmergencyManager.Instance)
                BunkerEmergencyManager.Instance.ChangeEmergencyTypeEvent -= OnChangeEmergencyType;
        }
    }
}