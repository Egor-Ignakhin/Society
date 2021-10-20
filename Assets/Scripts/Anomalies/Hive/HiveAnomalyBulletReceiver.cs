using Society.Patterns;
using Society.Shoot;

using UnityEngine;

namespace Society.Anomalies.Hive
{
    internal sealed class HiveAnomalyBulletReceiver : MonoBehaviour, IBulletReceiver
    {
        [SerializeField] private HiveAnomalyManager hiveAnomalyManager;
        public void OnBulletEnter(BulletType inputBulletType)
        {
            if (inputBulletType == BulletType.Electric)
                hiveAnomalyManager.Hit();
        }
    }
}