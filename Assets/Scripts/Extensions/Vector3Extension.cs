using UnityEngine;

namespace Society
{
    internal static class Vector3Extensions
    {
        public static Vector3 CalculateSpawnPositionInRange(Transform transform, Collider collider)
        {
            var width = collider.bounds.size.x;
            var center = transform.position;
            return new Vector3(GetRandomDot(center.x, width, 0),
                                GetRandomDot(center.y, width, 0),
                                GetRandomDot(center.z, width, 0));
        }

        private static float GetRandomDot(float center, float sideLength, float axisLowestLimit)
        {
            var LowestVertex = center + axisLowestLimit - (sideLength / 2);
            var HeighestVertex = center + (sideLength / 2);
            var RandomDot = Random.Range(LowestVertex, HeighestVertex);
            return RandomDot;
        }
    }
}