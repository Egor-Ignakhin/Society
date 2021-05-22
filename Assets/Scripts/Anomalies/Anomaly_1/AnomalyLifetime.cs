using System.Collections;
using UnityEngine;

namespace Anomaly_1
{
    class AnomalyLifetime : MonoBehaviour
    {
        [SerializeField] private float timeForDestroyInSeconds;
        private AnomalyPulsation anomalyPulsation;

        private void Start()
        {
            anomalyPulsation = GetComponent<AnomalyPulsation>();
        }

        public void MakePulsation(GameObject anomaly, Vector3 spawnPosition)
        {
            var instantiatedAnomaly = Instantiate(anomaly, spawnPosition, Quaternion.identity);
            anomalyPulsation.Pulsate(spawnPosition);
            StartCoroutine(DestroyAnomaly(instantiatedAnomaly));
        }

        private IEnumerator DestroyAnomaly(GameObject instantiatedAnomaly)
        {
            yield return new WaitForSeconds(timeForDestroyInSeconds);
            Destroy(instantiatedAnomaly);
        }
    }
}