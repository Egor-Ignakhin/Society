using System;
using UnityEngine;
namespace Features
{
    sealed class ShowerExample : InteractiveObject
    {
        [SerializeField] private ShowerGarden showerGarden;
        [SerializeField] private WaterContent waterContent = new WaterContent();
        [SerializeField] private ParticleSystem waterEffect;
        private bool waterIsEnable;
        public bool WaterIsEnable
        {
            get => waterIsEnable; set
            {
                var emission = waterEffect.emission;
                emission.enabled = value;
                waterIsEnable = value;
            }
        }
        internal bool ChangeEnableWater()
        {
            WaterIsEnable = !WaterIsEnable;
            return WaterIsEnable;
        }

        private void Start()
        {
            WaterIsEnable = false;
            SetType(nameof(ShowerExample));
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
                if (waterContent.WaterWeight >= 0)
                    PourOutWater();
                else WaterIsEnable = false;
            }
        }
        private void PourOutWater()
        {
            waterContent.WaterWeight -= Time.deltaTime;
            
            UpdateWaterDesc();
        }
        private void UpdateWaterDesc() =>
            additionalDescription = $" ({Math.Round(waterContent.WaterWeight, 1)}/{waterContent.MaxWaterWeight})";
        [System.Serializable]
        public sealed class WaterContent
        {
            public float WaterWeight;
            public float MaxWaterWeight = 20;
            public WaterContent()
            {
                WaterWeight = MaxWaterWeight;
            }
        }
    }
}