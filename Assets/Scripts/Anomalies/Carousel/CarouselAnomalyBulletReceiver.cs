using Society.Patterns;
using Society.Shoot;

using UnityEngine;

namespace Society.Anomalies.Carousel
{
    sealed class CarouselAnomalyBulletReceiver : AnomalyBulletReceiver
    {
        private bool needBlust;
        [SerializeField] private ParticleSystem aura;
        private float defaultStartSizeM;        
        private void Awake()
        {
            defaultStartSizeM = aura.main.startSpeedMultiplier;            
        }
        public override void OnBulletEnter(BulletType inputBulletType)
        {
            needBlust = true;
            ParticleSystem.MainModule main = aura.main;
            main.startSizeMultiplier += 2;
            manager.Hit(1);
        }
        private void FixedUpdate()
        {
            if (needBlust)
                AnimateBlust();
        }

        private void AnimateBlust()
        {
            ParticleSystem.MainModule main = aura.main;
            if ((main.startSizeMultiplier -= (Time.fixedDeltaTime * 100)) <= defaultStartSizeM)
                needBlust = false;
        }

        public override Transform GetCenter() => transform;
    }
}