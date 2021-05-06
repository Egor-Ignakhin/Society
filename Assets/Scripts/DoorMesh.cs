public sealed class DoorMesh : InteractiveObject//класс реализует взаимодействие с дверью
{
    private DoorManager mManager;
    protected override void Awake()
    {
        base.Awake();
        mManager = transform.parent.GetComponent<DoorManager>();        
        mManager.SetType(this);
    }
    public override void Interact(PlayerClasses.PlayerStatements pl)
    {
        mManager.Interact(this);
    }
}
