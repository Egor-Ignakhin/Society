using Society.Shoot;

using UnityEngine;

namespace Society.Anomalies.Hive
{
    internal sealed class HiveAnomalyBulletReceiver : AnomalyBulletReceiver
    {
        public override Transform GetCenter() => transform;

        public override void OnBulletEnter(BulletType inputBulletType)
        {
            manager.Hit(1);
        }
    }
}