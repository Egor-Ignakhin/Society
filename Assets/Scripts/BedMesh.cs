using PlayerClasses;
using UnityEngine;

public sealed class BedMesh : InteractiveObject
{
    [SerializeField] private BedManager mManager;
    public Transform SleepPlace;
    public bool IsOccupied { get; private set; }
    public override void Interact(PlayerStatements pl)
    {
        mManager.Interact(this);
        IsOccupied = true;
    }
    public void SetOccupied(bool value)
    {
        IsOccupied = value;
    }
}
