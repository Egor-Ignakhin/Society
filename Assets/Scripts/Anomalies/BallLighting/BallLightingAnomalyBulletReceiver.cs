using Society.Patterns;
using Society.Shoot;

using UnityEngine;

namespace Society.Anomalies.BallLighting
{
    public class BallLightingAnomalyBulletReceiver : MonoBehaviour, IBulletReceiver
    {
        [SerializeField] private BallLightingAnomalyManager manager;

        public Transform GetCenter() => transform;

        public void OnBulletEnter(BulletType inputBulletType)
        {
            manager.Hit(1);
        }
    }
}