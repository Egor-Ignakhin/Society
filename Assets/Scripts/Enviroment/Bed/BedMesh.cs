using Society.Patterns;

using UnityEngine;
namespace Society.Enviroment.Bed
{
    public sealed class BedMesh : InteractiveObject
    {
        [SerializeField] private BedManager mManager;
        [SerializeField] private Transform SleepPlace;
        private bool isOccupied;

        public bool GetIsOccupied() => isOccupied;

        private void SetIsOccupied(bool value) => isOccupied = value;

        public override void Interact()
        {
            mManager.StraightenBed(this);
            SetIsOccupied(true);
        }
        public void SetOccupied(bool value) => SetIsOccupied(value);

        public Transform GetSleepingPlace() => SleepPlace;
    }
}