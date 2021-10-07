using Society.Patterns;

using System.Collections;

using UnityEngine;

namespace Society.Anomalies.Anomaly_1
{
    sealed class AnomalyLifetime : ObjectPool
    {
        [SerializeField] private float timeForDestroyInSeconds;
        private AnomalyPulsation anomalyPulsation;

        public override void SetPrefabAsset(PoolableObject instance)
        {
            anomalyPulsation = GetComponent<AnomalyPulsation>();
            prefabAsset = instance;
            Preload();
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

        protected override int PreLoadedCount()
        {
            return 1;
        }
    }
}