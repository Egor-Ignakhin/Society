using Society.Patterns;
using Society.Player.Controllers;

using UnityEngine;
namespace Society.Enviroment.Bed
{
    internal sealed class BedManager : MonoBehaviour
    {
        private Transform playerCameraTransform;
        private BedController bedController;// игрок
        private Vector3 sleepAngles = new Vector3(-30, 90, 0);// положение во сне
        private Transform lastPlayerParent;
        [SerializeField]
        private BedAnimator mBedAnimator;
        private void Awake()
        {
            playerCameraTransform = Camera.main.transform;
            bedController = FindObjectOfType<BedController>();
        }

        /// <summary>
        /// заправить кровать
        /// </summary>
        /// <param name="b"></param>
        public void StraightenBed(BedMesh b)
        {
            if (b.GetIsOccupied())// если кровать занята
            {
                mBedAnimator.Deoccupied(lastPlayerParent, b);
                return;
            }
            //иначе если кровать свободна

            //перемещение позиций в спальное значение
            lastPlayerParent = playerCameraTransform.parent;
            playerCameraTransform.SetParent(b.GetSleepingPlace());

            playerCameraTransform.localEulerAngles = sleepAngles;

            playerCameraTransform.position = b.GetSleepingPlace().position;
            //конец перемещения позиций

            bedController.SetState(State.locked, this, b);
        }

        public void BedAnimatorAnimDeOccupied() => mBedAnimator.AnimDeoccipied();
    }
}