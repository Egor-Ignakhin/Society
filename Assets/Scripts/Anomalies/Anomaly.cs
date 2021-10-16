using UnityEngine;

namespace Society.Anomalies
{
    public abstract class Anomaly : MonoBehaviour
    {
        [SerializeField] protected int health;

        internal int GetHealth() => health;
    }
}