using UnityEngine;

namespace Society.Anomalies
{
    public abstract class Anomaly : MonoBehaviour
    {
        [SerializeField] protected int health;

        internal int GetHealth() => health;
        public abstract void Hit(int value);
        protected abstract void OnDie();
    }
}