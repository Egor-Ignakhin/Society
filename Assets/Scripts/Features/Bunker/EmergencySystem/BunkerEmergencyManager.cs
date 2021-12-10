using System;
using System.Collections.Generic;

using Society.Patterns;

using UnityEngine;

namespace Society.Features.Bunker.EmergencySystem
{
    public class BunkerEmergencyManager : Singleton<BunkerEmergencyManager>
    {
        [SerializeField] private EmergencyTypes emergencyType = EmergencyTypes.None;
        [SerializeField] private AudioSource mAudioSource;

        private Dictionary<EmergencyTypes, AudioClip> audioClipsForEmergencyTypes;

        public event Action<EmergencyTypes> ChangeEmergencyTypeEvent;

        private void Awake()
        {
            audioClipsForEmergencyTypes
            = new Dictionary<EmergencyTypes, AudioClip>
        {
                {EmergencyTypes.None, null },
                {EmergencyTypes.PowerProblems, Resources.Load<AudioClip>("Generator_gul")}
        };
        }

        private void Start()
        {
            SetEmergencyType(EmergencyTypes.None);
        }

        public void SetEmergencyType(EmergencyTypes et)
        {
            emergencyType = et;

            ChangeEmergencyTypeEvent?.Invoke(emergencyType);

            mAudioSource.clip = audioClipsForEmergencyTypes[emergencyType];
            mAudioSource.Play();
        }
    }
}