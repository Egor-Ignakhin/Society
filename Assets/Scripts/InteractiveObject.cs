using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    private string typeObject;   
    public static class Types
    {
        public const string OpenedDoor = "OpenedDoor";
        public const string ClosedDoor = "ClosedDoor";
        public const string LockedDoor = "LockedDoor";
    }
    protected virtual void Awake()
    {
        SetDescription();
    }
    public string GetTypeObject()
    {
        return typeObject;
    }
    public abstract void Interact(PlayerClasses.PlayerStatements pl);
    protected string description;
    public string GetDescription()
    {
        return description;
    }
    protected string LoadDescription()
    {        
        return Localization.GetHint(this);        
    }
    public void SetType(string t)
    {
        typeObject = t;
        SetDescription();
    }
    protected void SetDescription()
    {
        description = LoadDescription();
    }   
}
