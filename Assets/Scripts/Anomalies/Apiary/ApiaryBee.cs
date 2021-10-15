
using UnityEngine;
namespace Society.Anomalies.ApiaryAnomaly
{

    public sealed class ApiaryBee : MonoBehaviour
    {
        private Vector3 targetPosition;
        private ApiaryManager apiaryManager;

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
            transform.position = Vector3.MoveTowards(transform.position, GetTargetPosition(), Time.deltaTime);

            if (targetPosition == transform.position)
                SetTargetPosition(apiaryManager.RecalculateTargetPosition(transform));
        }
    }
}