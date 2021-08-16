using System;
using UnityEngine;
namespace Features
{
    sealed class ShowerExample : InteractiveObject
    {
        [SerializeField] private ShowerGarden showerGarden;
        private WaterContent MWaterContent { get; set; } = new WaterContent();
        [SerializeField] private ParticleSystem waterEffect;
        private bool waterIsEnable;
        public bool WaterIsEnable
        {
            get => waterIsEnable; set
            {
                var emission = waterEffect.emission;
                emission.enabled = value;
                SetEnableWaterAudioEffect(value);
                waterIsEnable = value;
            }
        }
        public bool ContentIsFilled => MWaterContent.WaterWeight >= MWaterContent.MaxWaterWeight;

        public bool IsReloading { get; internal set; }
        private AudioClip waterDropClip;
        private AudioClip moveClip;
        [SerializeField] private AudioSource mAudS;
        [SerializeField] private AudioSource movableAudioSource;        

        internal bool ChangeEnableWater()
        {
            WaterIsEnable = !WaterIsEnable;
            return WaterIsEnable;
        }

        private void Start()
        {
            waterDropClip = Resources.Load<AudioClip>("ShowerGarden\\WaterDrop");
            moveClip = Resources.Load<AudioClip>("ShowerGarden\\ShowerMove");
            WaterIsEnable = false;
            SetType(nameof(ShowerExample));
            UpdateWaterDesc();
        }

        internal void AddContentByTime()
        {
            MWaterContent.WaterWeight += Time.deltaTime;
            if (ContentIsFilled)
            {
                IsReloading = false;
            }
            UpdateWaterDesc();
        }

        public override void Interact()
        {
            showerGarden.OnInteract(this);
        }
        private void Update()
        {
            if (WaterIsEnable)
            {
                if (MWaterContent.WaterWeight >= 0)
                    PourOutWater();
                else WaterIsEnable = false;
            }
        }
        private void PourOutWater()
        {
            MWaterContent.WaterWeight -= Time.deltaTime;

            UpdateWaterDesc();
        }
        private void UpdateWaterDesc() =>
            additionalDescription = $" ({Math.Round(MWaterContent.WaterWeight, 1)}/{MWaterContent.MaxWaterWeight})";


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
        public void OnMove()
        {            
            if (movableAudioSource.isPlaying)
                return;

            movableAudioSource.Stop();
            movableAudioSource.clip = moveClip;
            movableAudioSource.Play();            
        }

        public sealed class WaterContent
        {
            public float WaterWeight { get; set; }
            public float MaxWaterWeight { get; set; } = 20;
            public WaterContent()
            {
                WaterWeight = MaxWaterWeight;
            }
        }
    }
}