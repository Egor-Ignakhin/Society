using System;
using System.Runtime.CompilerServices;

using Society.Enemies;
using Society.Player;

using UnityEngine;

namespace Society.Anomalies.BallLighting
{
    public sealed class BallLightingAnomalyManager : AnomalyManager
    {
        [SerializeField] private Path path = new Path();
        [SerializeField] private float movingSpeed = 1;
        [SerializeField] private AnomalyDieHandler dieHandler;
        [SerializeField] private float contactPower = 100;
        [SerializeField] private float blastPower = 500;
        private float dstTravelled;        

        public override void Hit(int value)
        {
            health -= value;
            if (health <= 0)
                OnDie();
        }

        protected override void OnDie()
        {
            gameObject.SetActive(false);
            dieHandler.OnInit();
        }

        private void FixedUpdate()
        {
            MoveToNextPoint();
        }

        internal float GetBlastPower() => blastPower;

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

        internal void DamageEnemy(EnemyCollision ec)
        {
            ec.InjureEnemy(contactPower * Time.fixedDeltaTime);
        }

        internal void InjurePlayer(BasicNeeds bn)
        {
            bn.InjurePerson(contactPower * Time.fixedDeltaTime);
        }
    }
}