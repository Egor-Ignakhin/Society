using System.Collections.Generic;

using Society.Enemies;
using Society.Player;

using UnityEngine;
using UnityEngine.VFX;

namespace Society.Anomalies.BallLighting
{
    public sealed class BallLightingAnomalyLine : MonoBehaviour
    {
        private float radius = 3;
        [SerializeField] private Transform lineTargetPoint;

        private VisualEffect mVisualEffect;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private BallLightingAnomalyManager anomalyManager;
        private void Awake()
        {
            mVisualEffect = GetComponent<VisualEffect>();
        }
        private void FixedUpdate()
        {
            SetupTargetPoint(
                FindNearestPhysicalPoint());
        }

        private Vector3 FindNearestPhysicalPoint()
        {
            Vector3 center = transform.position;

            Collider[] colliders = Physics.OverlapSphere(center, radius, layerMask, QueryTriggerInteraction.Ignore);
            List<Collider> bscOrMColliders = new List<Collider>();


            foreach (var collider in colliders)
            {
                if (collider as BoxCollider || collider as SphereCollider
                    || collider as CapsuleCollider ||
                    (collider as MeshCollider && (collider as MeshCollider).convex))
                    bscOrMColliders.Add(collider);
            }
            //Sort

            Vector3 foundedPoint = center;
            float minDist = float.MaxValue;

            for (int i = 0; i < bscOrMColliders.Count; i++)
            {
                float tempMinDist = Vector3.Distance(center, bscOrMColliders[i].ClosestPoint(center));

                if (bscOrMColliders[i].TryGetComponent(out EnemyCollision e))
                {
                    foundedPoint = bscOrMColliders[i].ClosestPoint(center);

                    anomalyManager.DamageEnemy(e);

                    break;
                }
                else if (bscOrMColliders[i].TryGetComponent(out BasicNeeds bn))
                {
                    foundedPoint = bscOrMColliders[i].ClosestPoint(center);

                    anomalyManager.InjurePlayer(bn);

                    break;
                }

                if (tempMinDist < minDist)
                {
                    minDist = tempMinDist;
                    foundedPoint = bscOrMColliders[i].ClosestPoint(center);
                }
            }

            mVisualEffect.enabled = bscOrMColliders.Count != 0;

            return foundedPoint;
        }

        private void SetupTargetPoint(Vector3 target)
        {
            lineTargetPoint.position = target;

            transform.LookAt(target);
        }
    }
}