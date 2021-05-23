using System.Collections;
using UnityEngine;

namespace Anomaly_1
{
    class AnomalyScheduler : MonoBehaviour
    {
        [SerializeField] private float lowerTimeLimitInSeconds; // Минимальное время до появление аномалии
        [SerializeField] private float upperTimeLimitInSeconds;
        [SerializeField] private PoolableObject anomaly;
        private SpawnPositionFinder spawnPositionFinder;
        private AnomalyLifetime anomalyLifetime;

        private void Start()
        {
            spawnPositionFinder = GetComponent<SpawnPositionFinder>();
            anomalyLifetime = GetComponent<AnomalyLifetime>();
            var transform = GetComponent<Transform>();
            var collider = GetComponent<Collider>();

            anomalyLifetime.OnInit(anomaly);
            StartCoroutine(SetSpawn(CalculateWaitTime(lowerTimeLimitInSeconds, upperTimeLimitInSeconds), transform, collider));
        }

        private IEnumerator SetSpawn(float waitTimeInSeconds, Transform transform, Collider collider)
        {
            yield return new WaitForSeconds(waitTimeInSeconds);
            anomalyLifetime.MakePulsation(spawnPositionFinder.CalculateSpawnPosition(transform, collider));
            StartCoroutine(SetSpawn(CalculateWaitTime(lowerTimeLimitInSeconds, upperTimeLimitInSeconds), transform, collider));
        }

        private float CalculateWaitTime(float lowerTimeLimitInSeconds, float upperTimeLimitInSeconds)
        {
            return Random.Range(lowerTimeLimitInSeconds, upperTimeLimitInSeconds);
        }
    }
}