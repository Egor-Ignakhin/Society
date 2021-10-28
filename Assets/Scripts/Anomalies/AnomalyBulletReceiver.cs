
using Society.Patterns;
using Society.Shoot;

using UnityEngine;

namespace Society.Anomalies
{
    public abstract class AnomalyBulletReceiver : MonoBehaviour, IBulletReceiver
    {
        [SerializeField] protected AnomalyManager manager;

        public abstract Transform GetCenter();

        public abstract void OnBulletEnter(BulletType inputBulletType);
    }
}