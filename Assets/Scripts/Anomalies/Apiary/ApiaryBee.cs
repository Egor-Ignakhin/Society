
using System;

using UnityEngine;
namespace Society.Anomalies.Apiary
{

    public sealed class ApiaryBee : MonoBehaviour
    {
        private Vector3 targetPosition;
        private ApiaryManager apiaryManager;
        private float speedFly = 2;

        internal void OnInit(ApiaryManager am)
        {
            apiaryManager = am;
            targetPosition = transform.position;
        }

        public Vector3 GetTargetPosition()
        {
            return targetPosition;
        }

        public void SetTargetPosition(Vector3 value)
        {
            targetPosition = value;
        }

        private void Update()
        {
            MoveToTargetPosition();
        }

        private void MoveToTargetPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position, GetTargetPosition(), Time.deltaTime * speedFly);

            if (targetPosition == transform.position)
                SetTargetPosition(apiaryManager.RecalculateTargetPosition());
        }

        internal float AttackDistance() => 0.1f;

        internal float Damage() => 0.5f;
    }
}