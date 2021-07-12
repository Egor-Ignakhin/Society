using UnityEngine;

sealed class CallInteractObject : InteractiveObject
{
    [SerializeField] private InteractiveObject receiver;

    public override void Interact() => receiver.Interact();
}
