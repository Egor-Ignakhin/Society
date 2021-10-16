using UnityEngine;

namespace Features
{
    internal sealed class ShowerLeverOnWall : Society.Patterns.InteractiveObject
    {
        [SerializeField] private ShowerExample showerExample;
        [SerializeField] private ParticleSystem waterParticleSystem;
        [SerializeField] private AudioSource mAudS;


        public bool IsLeverOpen { get; private set; }

        public void SetIsLeverOpen(bool value)
        {
            IsLeverOpen = value;

            var em = waterParticleSystem.emission;
            em.enabled = value;
            SetType(!value ? "ClosedShowerLever" : "OpenedShowerLever");
            SetEnableWaterAudioEffect(value);
        }

        private AudioClip waterAudio;
        private void Start()
        {
            waterAudio = Resources.Load<AudioClip>("ShowerGarden\\WaterDrop");
            SetIsLeverOpen(false);
        }

        public override void Interact()
        {
            SetIsLeverOpen(!IsLeverOpen);
        }
        private void Update()
        {
            if (IsLeverOpen && (!showerExample.GetIsFull()))
            {
                showerExample.SetWaterVolume(showerExample.GetWaterVolume() + Time.deltaTime);
            }
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
    }
}