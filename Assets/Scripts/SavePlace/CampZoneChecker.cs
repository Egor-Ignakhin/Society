using UnityEngine;

namespace BarrelCampScripts
{
    sealed class CampZoneChecker : MonoBehaviour// класс отвечающий за проверку входа и выхода игрока из зоны лагеря
    {
        [SerializeField] private BarrelCampManager campManager;// менеджер лагеря
        private CapsuleCollider playerCollider;// коллизия игрока
        private void Start()
        {
            playerCollider = FirstPersonController.GetCollider();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other == playerCollider)
            {
                campManager.InsidePlayer();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other == playerCollider)
            {
                campManager.OutsidePlayer();
            }
        }
    }
}