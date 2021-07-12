using UnityEngine;

public sealed class ChairMesh : InteractiveObject
{
    [SerializeField] private ChairManager mManager;
    [SerializeField] private Transform SeatPlace;

    public bool IsOccupied { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        SetType("Сhair");
    }

    public void SetOccupied(bool value) => IsOccupied = value;

    public override void Interact()
    {
        mManager.Interact(this);
        IsOccupied = true;
    }
    public Transform GetSeatPlace() => SeatPlace;
}
