public sealed class CupboardMesh : InteractiveObject
{
    [UnityEngine.SerializeField] private CupboardManager mManager;
    public bool IsOpen { get; private set; }// открыт ли ящик
    private bool canInteract { get; set; } = true;// возможность взаимодействия
    protected override void Awake()
    {
        base.Awake();
        mManager.AddCase(this);
    }
    public override void Interact(PlayerClasses.PlayerStatements pl)
    {
        if (!canInteract)
            return;
        mManager.Interact(transform);
        canInteract = false;
    }
    public void SetOpened(bool value)
    {
        IsOpen = value;
        canInteract = true;
    }
}
