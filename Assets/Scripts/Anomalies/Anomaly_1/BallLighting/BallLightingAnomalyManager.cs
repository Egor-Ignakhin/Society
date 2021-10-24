using System;
using System.Runtime.CompilerServices;

using UnityEngine;

namespace Society.Anomalies.BallLighting
{
    public sealed class BallLightingAnomalyManager : Anomaly
    {
        [SerializeField] private Path path = new Path();
        [SerializeField] private float movingSpeed = 1;
        private float dstTravelled;

        private void FixedUpdate()
        {
            MoveToNextPoint();
        }

        private void MoveToNextPoint()
        {
            dstTravelled += movingSpeed * Time.deltaTime;
            transform.position = path.GetPointAtDistance(dstTravelled);                       
        }

        [System.Serializable]
        public class Path
        {
            [SerializeField] private PathCreation.PathCreator pathCreator;
            private int currentPointerIndex;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Vector3 GetPointAtDistance(float dstTravelled)
            {
                return pathCreator.path.GetPointAtDistance(dstTravelled, PathCreation.EndOfPathInstruction.Loop);
            }
        }
    }
}