using UnityEngine;

namespace Society.Features.Bunker.EmergencySystem
{
    internal sealed class Lamp : MonoBehaviour
    {
        [SerializeField] private Light mLight;
        [SerializeField] private GameObject activePlith;
        [SerializeField] private GameObject dontActivePlith;

        private void OnEnable()
        {
            BunkerEmergencyManager.Instance.ChangeEmergencyTypeEvent += OnChangeEmergencyType;
        }


        private void OnChangeEmergencyType(EmergencyTypes emergencyType)
        {
            if (emergencyType == EmergencyTypes.None)
            {
                mLight.gameObject.SetActive(true);
                activePlith.SetActive(true);
                dontActivePlith.SetActive(false);
            }
            else
            {
                mLight.gameObject.SetActive(false);
                activePlith.SetActive(false);
                dontActivePlith.SetActive(true);
            }            
        }


        private void OnDisable()
        {
            if (BunkerEmergencyManager.Instance)
                BunkerEmergencyManager.Instance.ChangeEmergencyTypeEvent -= OnChangeEmergencyType;
        }

    }
}