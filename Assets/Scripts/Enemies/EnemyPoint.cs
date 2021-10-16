using UnityEngine;
namespace Society.Enemies
{
    internal sealed class EnemyPoint : MonoBehaviour
    {
        [SerializeField] private float timeDelay;
        private void Start()
        {
            ResetDelay();
        }
        public float GetDelay()
        {
            currentDelay -= Time.deltaTime;
            return currentDelay;
        }
        private float currentDelay;

        internal void ResetDelay()
        {
            currentDelay = timeDelay;
        }
    }
}