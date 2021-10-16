
using Society.Player;

using UnityEngine;
namespace Society.Anomalies
{
    internal sealed class RadiationZone : MonoBehaviour
    {
        [SerializeField] private float powerZone = 1;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<BasicNeeds>(out var basicNeeds))
                basicNeeds.SetIsInsadeZone(true);
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<BasicNeeds>(out var basicNeeds))
            {
                float m = Vector3.Distance(transform.position, other.transform.position);
                if (m > 0)
                    m = 1 / m;

                basicNeeds.AddRadiation(powerZone * m * Time.deltaTime);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<BasicNeeds>(out var basicNeeds))
                basicNeeds.SetIsInsadeZone(false);
        }
    }
}