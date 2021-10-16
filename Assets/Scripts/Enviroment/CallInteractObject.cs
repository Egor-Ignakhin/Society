using Society.Patterns;

using UnityEngine;
namespace Society.Enviroment
{
    internal sealed class CallInteractObject : InteractiveObject
    {
        [SerializeField] private InteractiveObject receiver;

        public override void Interact() => receiver.Interact();
    }
}