using System;

using UnityEngine;
namespace Features
{
    internal sealed class ShowerExample : Society.Patterns.InteractiveObject
    {
        [SerializeField] private ShowerManager showerGarden;
        [SerializeField] private AudioSource mAudS;
        [SerializeField] private AudioSource movableAudioSource;
        [SerializeField] private Transform weightArrow;
        [SerializeField] private ParticleSystem waterParticleSystem;

        private AudioClip waterAudio;
        private AudioClip moveAudio;

        public bool IsFull;// => waterContent.WaterWeight >= waterContent.MaxWaterWeight; 

        public bool GetIsFull() => IsFull;
        public void SetIsFull(bool v) => IsFull = v;

        private bool isWaterOpen;

        public event Action<float> ChangeWaterVolumeEvent;

        private float waterVolume;
        public float MaxWaterVolume { get; set; } = 20;

        public float GetWaterVolume()
        {
            return waterVolume;
        }

        public void SetWaterVolume(float value)
        {
            ChangeWaterVolumeEvent?.Invoke(value);
            waterVolume = value;
        }

        private void SetIsWaterOpen(bool value)
        {
            isWaterOpen = value;

            var waterEmission = waterParticleSystem.emission;
            waterEmission.enabled = value;
            SetEnableWaterAudioEffect(value);
        }

        internal bool ChangeIsWaterOpen()
        {
            SetIsWaterOpen(!isWaterOpen);
            return isWaterOpen;
        }

        private void Start()
        {
            waterAudio = Resources.Load<AudioClip>("ShowerGarden\\WaterDrop");
            moveAudio = Resources.Load<AudioClip>("ShowerGarden\\ShowerMove");
            SetIsWaterOpen(false);
            SetType(nameof(ShowerExample));
            ChangeWaterVolumeEvent += OnChangeWaterWeight;

            SetWaterVolume(MaxWaterVolume);

        }

        public override void Interact()
        {
            showerGarden.OnInteract(this);
        }
        private void Update()
        {
            if (isWaterOpen)
            {
                if (GetWaterVolume() >= 0)
                    SetWaterVolume(GetWaterVolume() - Time.deltaTime);
                else
                    SetIsWaterOpen(false);
            }
        }

        private void OnChangeWaterWeight(float v)
        {
            var angles = weightArrow.localEulerAngles;
            float maxAngle = 220;
            angles.y = maxAngle * v / MaxWaterVolume;
            weightArrow.localRotation = Quaternion.Euler(angles);
        }


        private void SetEnableWaterAudioEffect(bool isEnabled)
        {
            if (isEnabled)
            {
                mAudS.clip = waterAudio;
                mAudS.Play();
            }
            else
            {
                mAudS.Stop();
                mAudS.clip = null;
            }
        }
        public void OnMove()
        {
            if (movableAudioSource.isPlaying)
                return;

            movableAudioSource.Stop();
            movableAudioSource.clip = moveAudio;
            movableAudioSource.Play();
        }

        private void OnDisable() =>
            ChangeWaterVolumeEvent = null;

    }
}