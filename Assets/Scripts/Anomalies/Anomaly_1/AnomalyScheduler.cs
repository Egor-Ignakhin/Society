using Society.Patterns;

using System.Collections;

using UnityEngine;

namespace Society.Anomalies.Anomaly_1
{
    internal sealed class AnomalyScheduler : MonoBehaviour
    {
        [SerializeField] private float lowerTimeLimitInSeconds; // Минимальное время до появление аномалии
        [SerializeField] private float upperTimeLimitInSeconds;
        [SerializeField] private PoolableObject anomaly;        
        private AnomalyLifetime anomalyLifetime;

        private void Start()
        {            
            anomalyLifetime = GetComponent<AnomalyLifetime>();
            var transform = GetComponent<Transform>();
            var collider = GetComponent<Collider>();

            anomalyLifetime.SetPrefabAsset(anomaly);
            StartCoroutine(SetSpawn(CalculateWaitTime(lowerTimeLimitInSeconds, upperTimeLimitInSeconds), transform, collider));
        }

        private IEnumerator SetSpawn(float waitTimeInSeconds, Transform transform, Collider collider)
        {
            yield return new WaitForSeconds(waitTimeInSeconds);            
            anomalyLifetime.MakePulsation(Extensions.CalculateSpawnPositionInRange(transform,collider));
            StartCoroutine(SetSpawn(CalculateWaitTime(lowerTimeLimitInSeconds, upperTimeLimitInSeconds), transform, collider));
        }

        private float CalculateWaitTime(float lowerTimeLimitInSeconds, float upperTimeLimitInSeconds) =>
            Random.Range(lowerTimeLimitInSeconds, upperTimeLimitInSeconds);
    }
}