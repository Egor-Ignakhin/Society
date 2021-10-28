using Society.Shoot;

using UnityEngine;

namespace Society.Anomalies.BallLighting
{
    internal sealed class BallLightingAnomalyBulletReceiver : AnomalyBulletReceiver
    {
        public override Transform GetCenter() => transform;

        public override void OnBulletEnter(BulletType inputBulletType) => manager.Hit(1);
    }
}