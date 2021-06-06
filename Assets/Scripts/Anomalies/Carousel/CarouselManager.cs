using System;
using System.Collections.Generic;
using UnityEngine;

namespace CarouselAnomaly
{
    public class CarouselManager : MonoBehaviour
    {
        private readonly List<Rigidbody> items = new List<Rigidbody>();
        private Collider mCollider;
        private Vector3 PointPos;
        private Vector3 targetPos = Vector3.zero;
        private BoxCollider zone;
        public void InitCollider(Collider c)
        {
            mCollider = c;
            PointPos = mCollider.transform.position;
            PointPos.y = (mCollider as CapsuleCollider).height;
        }

        private System.Collections.IEnumerator Start()
        {
            while (true)
            {
                targetPos = CalculateSpawnPosition(zone.transform, zone);
                yield return new WaitForSeconds(4);
            }
        }
        internal void SetZone(BoxCollider mCollider) => zone = mCollider;

        public Vector3 CalculateSpawnPosition(Transform transform, Collider collider)
        {
            var width = collider.bounds.size.x;
            var center = transform.position;
            return new Vector3(GetRandomDot(center.x, width, 0),
                                0,
                                GetRandomDot(center.z, width, 0));
        }
        private float GetRandomDot(float center, float sideLength, float axisLowestLimit)
        {
            var LowestVertex = center + axisLowestLimit - (sideLength / 2);
            var HeighestVertex = center + (sideLength / 2);
            var RandomDot = UnityEngine.Random.Range(LowestVertex, HeighestVertex);
            return RandomDot;
        }

        public void AddInList(Rigidbody rb) => items.Add(rb);

        public void RemoveFromList(Rigidbody rb) => items.Remove(rb);

        private void FixedUpdate()
        {
            Vector3 itemPos;
            for (int i = 0; i < items.Count; i++)
            {
                itemPos = items[i].transform.position;
                float m = Vector3.Distance(PointPos, itemPos);
                m *= 1 / m;
                items[i].AddForce((PointPos - itemPos).normalized * m, ForceMode.VelocityChange);
                items[i].angularVelocity += new Vector3(5, 0, 0) * Time.deltaTime;
                if (itemPos.y > (PointPos.y - 1))
                {
                    items[i].AddForce(-(PointPos - itemPos).normalized * m * 100, ForceMode.VelocityChange);
                }
            }
            MoveToTarget();
        }
        private void MoveToTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.fixedDeltaTime);
        }
    }
}