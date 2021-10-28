using Society.Patterns;
using Society.Shoot;

using UnityEngine;

namespace Society.Anomalies.Carousel
{
    internal class CarouselBulletsTrigger : MonoBehaviour, IBulletReceiver
    {
        private bool needBlust;
        [SerializeField] private ParticleSystem aura;
        private float defaultStartSizeM;
        private CarouselManager cm;
        private void Awake()
        {
            defaultStartSizeM = aura.main.startSpeedMultiplier;
            cm = gameObject.transform.parent.parent.gameObject.GetComponent<CarouselManager>();
        }
        public void OnBulletEnter(BulletType inputBulletType)
        {
            needBlust = true;
            ParticleSystem.MainModule main = aura.main;
            main.startSizeMultiplier += 2;
            cm.Hit(1);
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

        public Transform GetCenter()
        {
            return transform;
        }
    }
}