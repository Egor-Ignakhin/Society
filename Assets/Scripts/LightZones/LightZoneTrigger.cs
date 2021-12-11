using Society.Player;

using UnityEngine;

namespace Society.LightZones
{
    internal sealed class LightZoneTrigger : MonoBehaviour
    {
        private LightZoneManager lightZoneManager;

        private void Awake()
        {
            transform.parent.TryGetComponent(out lightZoneManager);
            lightZoneManager.LightZoneTriggers.Add(this, false);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<BasicNeeds>())
                lightZoneManager.LightZoneTriggers[this] = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<BasicNeeds>())
                lightZoneManager.LightZoneTriggers[this] = false;
        }
    }
}