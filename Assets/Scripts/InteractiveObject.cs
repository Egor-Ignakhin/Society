using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public string Type { get; private set; }    
    public static class Types
    {
        public const string OpenedDoor = "OpenedDoor";
        public const string ClosedDoor = "ClosedDoor";
        public const string LockedDoor = "LockedDoor";        
    }
    protected virtual void Awake() => SetDescription();

    public abstract void Interact(PlayerClasses.PlayerStatements pl);
    public string Description { get; protected set; }
    public string MainDescription { get; protected set; } = string.Empty;

    public void SetType(string t)
    {
        Type = t;
        SetDescription();
    }
    protected void SetDescription() => Description = Localization.GetHint(this);

}
