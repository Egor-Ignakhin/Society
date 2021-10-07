using Society.Patterns;

using UnityEngine;
namespace Society.Enviroment.Bed
{
    public sealed class BedMesh : InteractiveObject
    {
        [SerializeField] private BedManager mManager;
        [SerializeField] private Transform SleepPlace;

        public bool IsOccupied { get; private set; }
        public override void Interact()
        {
            mManager.StraightenBed(this);
            IsOccupied = true;
        }
        public void SetOccupied(bool value) => IsOccupied = value;

        public Transform GetSleepPlace() => SleepPlace;
    }
}