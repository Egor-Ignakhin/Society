using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    protected PlayerClasses.PlayerInteractive playerInteractive;
    private string description;
    public string Description { get => $"{description}{additionalDescription}"; protected set => description = value; }
    protected string additionalDescription;
    public string Type { get; private set; }

    public enum Types { OpenedDoor, ClosedDoor, LockedDoor, Container_1 };

    protected virtual void Awake()
    {
        SetDescription();
        playerInteractive = FindObjectOfType<PlayerClasses.PlayerInteractive>();
    }

    public abstract void Interact();
    public string MainDescription { get; protected set; } = string.Empty;

    public void SetType(string t)
    {
        Type = t;
        SetDescription();
    }
    protected void SetDescription() => Description = Localization.GetHint(this);

}
