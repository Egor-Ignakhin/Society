using UnityEngine;
namespace ApiaryAnomaly
{
    public sealed class ApiaryManager : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(0, 100 * Time.deltaTime, 0);
        }
    }
}