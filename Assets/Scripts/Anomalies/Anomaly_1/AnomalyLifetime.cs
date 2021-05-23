using System.Collections;
using UnityEngine;

namespace Anomaly_1
{
    class AnomalyLifetime : ObjectPool
    {
        [SerializeField] private float timeForDestroyInSeconds;
        private AnomalyPulsation anomalyPulsation;

        public void OnInit(PoolableObject instance)
        {
            anomalyPulsation = GetComponent<AnomalyPulsation>();
            prefabAsset = instance;
        }

        public void MakePulsation(Vector3 spawnPosition)
        {
            var instantiatedAnomaly = GetObjectFromPool();
            instantiatedAnomaly.transform.position = spawnPosition;
            anomalyPulsation.Pulsate(spawnPosition);
            StartCoroutine(DestroyAnomaly(instantiatedAnomaly));
        }

        private IEnumerator DestroyAnomaly(PoolableObject instantiatedAnomaly)
        {
            yield return new WaitForSeconds(timeForDestroyInSeconds);
            ReturnToPool(instantiatedAnomaly);
        }
    }
}