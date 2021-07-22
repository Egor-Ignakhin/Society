using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    protected PlayerClasses.PlayerInteractive playerInteractive;
    public string Type { get; private set; }

    public enum Types { OpenedDoor, ClosedDoor, LockedDoor, Container_1 };

    protected virtual void Awake()
    {
        SetDescription();
        playerInteractive = FindObjectOfType<PlayerClasses.PlayerInteractive>();
    }

    public abstract void Interact();
    public string Description { get; protected set; }
    public string MainDescription { get; protected set; } = string.Empty;

    public void SetType(string t)
    {
        Type = t;
        SetDescription();
    }
    protected void SetDescription() => Description = Localization.GetHint(this);

}
