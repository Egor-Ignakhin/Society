using Society.Player;

using UnityEngine;
namespace Society.Patterns
{
    public abstract class InteractiveObject : MonoBehaviour
    {
        protected PlayerInteractive playerInteractive;
        private string description;
        public string Description { get => $"{description}{additionalDescription}"; protected set => description = value; }
        protected string additionalDescription;
        public string Type { get; private set; }

        public enum Types { OpenedDoor, ClosedDoor, LockedDoor, Container_1 };

        protected virtual void Awake()
        {
            SetDescription();
            playerInteractive = FindObjectOfType<PlayerInteractive>();
        }

        public abstract void Interact();
        public string MainDescription { get; protected set; } = string.Empty;

        public void SetType(string t)
        {
            Type = t;
            SetDescription();
        }
        protected void SetDescription() => Description = Society.Localization.LocalizationManager.GetHint(Type);

    }
}