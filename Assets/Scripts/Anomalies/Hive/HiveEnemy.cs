using UnityEngine;
namespace Society.Anomalies.Hive
{

    public sealed class HiveEnemy : MonoBehaviour
    {
        private Vector3 targetPosition;
        private HiveAnomalyManager hiveAnomalyManager;
        private readonly float speedFlying = 2;

        internal void OnInit(HiveAnomalyManager am)
        {
            hiveAnomalyManager = am;
            targetPosition = transform.position;
        }

        public Vector3 GetTargetPosition() => targetPosition;

        public void SetTargetPosition(Vector3 value) => targetPosition = value;

        private void Update() => MoveToTargetPosition();

        private void MoveToTargetPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, GetTargetPosition(), Time.deltaTime * speedFlying);

            if (targetPosition == transform.position)
                SetTargetPosition(hiveAnomalyManager.RecalculateTargetPosition());
        }

        internal float AttackDistance() => 0.1f;

        internal float Damage() => 0.5f;
    }
}