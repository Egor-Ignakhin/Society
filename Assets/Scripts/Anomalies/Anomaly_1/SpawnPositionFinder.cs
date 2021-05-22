using UnityEngine;
namespace Anomaly_1
{
    class SpawnPositionFinder : MonoBehaviour
    {
        [SerializeField] private float yLowestLimit; // Ниже это высоты аномалия не будет появляться.

        public Vector3 CalculateSpawnPosition(Transform transform, Collider collider)
        {
            var width = collider.bounds.size.x;
            var height = collider.bounds.size.y;
            var center = transform.position;
            return new Vector3(GetRandomDot(center.x, width, 0),
                                GetRandomDot(center.y, height, yLowestLimit),
                                GetRandomDot(center.z, width, 0));
        }

        private float GetRandomDot(float center, float sideLength, float axisLowestLimit)
        {
            var LowestVertex = center + axisLowestLimit - (sideLength / 2);
            var HeighestVertex = center + (sideLength / 2);
            var RandomDot = Random.Range(LowestVertex, HeighestVertex);
            return RandomDot;
        }
    }
}