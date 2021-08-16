using UnityEngine;

namespace Features
{
    sealed class ShowerLeverOnWall : InteractiveObject
    {
        private bool leverIsOpened = false;
        [SerializeField] private ShowerExample showerExample;

        public bool LeverIsOpened
        {
            get => leverIsOpened; set
            {
                var em = waterEffect.emission;
                em.enabled = value;
                SetType(!value ? "ClosedShowerLever" : "OpenedShowerLever");
                SetEnableWaterAudioEffect(value);
                leverIsOpened = value;
            }
        }
        [SerializeField] private ParticleSystem waterEffect;
        [SerializeField] private AudioSource mAudS;

        private AudioClip waterDropClip;
        private void Start()
        {
            waterDropClip = Resources.Load<AudioClip>("ShowerGarden\\WaterDrop");
            LeverIsOpened = false;
        }

        public override void Interact()
        {
            LeverIsOpened = !LeverIsOpened;
        }
        private void Update()
        {
            if (LeverIsOpened && showerExample.IsReloading)
            {
                showerExample.AddContentByTime();
            }
        }
        private void SetEnableWaterAudioEffect(bool isEnabled)
        {
            if (isEnabled)
            {
                mAudS.clip = waterDropClip;
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