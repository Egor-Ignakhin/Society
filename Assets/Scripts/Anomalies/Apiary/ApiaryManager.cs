using System;

using UnityEngine;
namespace Society.Anomalies.ApiaryAnomaly
{
    internal sealed class ApiaryManager : MonoBehaviour
    {
        [SerializeField] private ApiaryBee beeInstansce;
        [SerializeField] private BoxCollider boxColliderSpawnRange;
        [SerializeField] private float yLowestLimit; // Ниже это высоты аномалия не будет появляться.

        private void Update()
        {
            transform.Rotate(0, 100 * Time.deltaTime, 0);

            GenerateBees();
        }

        private void GenerateBees()
        {
            Instantiate(beeInstansce, transform.position, Quaternion.identity).OnInit(this);
        }

        internal Vector3 RecalculateTargetPosition(Transform transform)
        {
            return CalculateSpawnPosition(transform);
        }        

        public Vector3 CalculateSpawnPosition(Transform transform)
        {
            var width = boxColliderSpawnRange.bounds.size.x;
            var height = boxColliderSpawnRange.bounds.size.y;
            var center = transform.position;
            return new Vector3(GetRandomDot(center.x, width, 0),
                                GetRandomDot(center.y, height, yLowestLimit),
                                GetRandomDot(center.z, width, 0));
        }

        private float GetRandomDot(float center, float sideLength, float axisLowestLimit)
        {
            var LowestVertex = center + axisLowestLimit - (sideLength / 2);
            var HeighestVertex = center + (sideLength / 2);
            var RandomDot = UnityEngine.Random.Range(LowestVertex, HeighestVertex);
            return RandomDot;
        }
    }
}