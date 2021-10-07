using UnityEngine;
namespace Society.Anomalies.ApiaryAnomaly
{
    sealed class ApiaryManager : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(0, 100 * Time.deltaTime, 0);
        }
    }
}