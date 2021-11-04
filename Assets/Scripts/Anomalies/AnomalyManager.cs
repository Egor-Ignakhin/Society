using UnityEngine;

namespace Society.Anomalies
{
    public abstract class AnomalyManager : MonoBehaviour
    {
        [SerializeField] protected int health;

        internal int GetHealth() => health;
        public abstract void Hit(int value);
        protected abstract void OnDie();
    }
}