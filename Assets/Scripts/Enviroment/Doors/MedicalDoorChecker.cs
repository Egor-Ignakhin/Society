using Society.Player.Controllers;

using UnityEngine;
namespace Society.Enviroment.Doors
{
    sealed class MedicalDoorChecker : MonoBehaviour
    {
        [SerializeField] private MedicalDoor myMedicalDoor;
        private Collider playerCollider;
        private void Start()
        {
            playerCollider = FindObjectOfType<FirstPersonController>().GetCollider();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other == playerCollider)
                myMedicalDoor.OnEnterPlayer();
        }
        private void OnTriggerExit(Collider other)
        {
            if (other == playerCollider)
                myMedicalDoor.OnExitPlayer();
        }
    }
}