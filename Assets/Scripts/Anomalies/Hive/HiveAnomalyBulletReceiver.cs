using Society.Patterns;

using UnityEngine;

namespace Society.Anomalies.Hive
{
    internal sealed class HiveAnomalyBulletReceiver : MonoBehaviour, IBulletReceiver
    {
        [SerializeField] private HiveAnomalyManager hiveAnomalyManager;
        public void OnBulletEnter() => hiveAnomalyManager.Hit();
    }
}