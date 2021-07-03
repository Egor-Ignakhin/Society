using UnityEngine;

public sealed class CupboardMesh : InteractiveObject
{
    [SerializeField] private CupboardManager mManager;
    public bool IsOpen { get; private set; }// открыт ли ящик
    private bool canInteract = true;// возможность взаимодействия
    [SerializeField] [Range(0, 10)] private float speed = 1;
    [SerializeField] private bool isLockedCupboard;
    protected override void Awake()
    {
        base.Awake();
        mManager.AddCase(this);
        SetOpened(IsOpen);
    }
    public override void Interact(PlayerClasses.PlayerStatements pl)
    {
        if (!canInteract || isLockedCupboard)
            return;
        mManager.Interact(transform, speed);
        canInteract = false;
    }
    public void SetOpened(bool value)
    {
        IsOpen = value;
        canInteract = true;
        if (!isLockedCupboard)
            SetType(IsOpen ? "OpenedBox" : "ClosedBox");
        else 
            SetType("LockedBox");
    }
}
