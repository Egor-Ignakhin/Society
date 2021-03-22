using PlayerClasses;
using UnityEngine;

public class ChairMesh : InteractiveObject
{
    [SerializeField] private ChairManager mManager;
    public Transform SeatPlace;
    [SerializeField] private float timeMultiply = 1;

    public bool IsOccupied { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        if (timeMultiply == 0)
            Debug.LogError("time is stopped!");
    }

    public void SetOccupied(bool value)
    {
        IsOccupied = value;
    }
    public override void Interact(PlayerStatements pl)
    {
        mManager.Interact(this);
        IsOccupied = true;
    }
    public float GetTimeMultiply()
    {
        return timeMultiply;
    }
}
