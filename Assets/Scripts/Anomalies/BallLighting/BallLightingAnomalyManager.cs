using System.Runtime.CompilerServices;

using UnityEngine;

namespace Society.Anomalies.BallLighting
{
    public sealed class BallLightingAnomalyManager : Anomaly
    {
        [SerializeField] private Path path = new Path();
        [SerializeField] private float movingSpeed = 1;
        [SerializeField] private BallLightingAnomalyDieHandler dieHandler;
        private float dstTravelled;


        public override void Hit(int value)
        {
            health -= value;                        
            if (health <= 0)
                OnDie();
        }

        protected override void OnDie()
        {
            dieHandler.transform.SetParent(null);
            gameObject.SetActive(false);
            dieHandler.gameObject.SetActive(true);
        }

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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Vector3 GetPointAtDistance(float dstTravelled)
            {
                return pathCreator.path.GetPointAtDistance(dstTravelled, PathCreation.EndOfPathInstruction.Loop);
            }
        }
    }
}