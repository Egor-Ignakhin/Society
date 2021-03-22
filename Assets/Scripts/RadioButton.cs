using PlayerClasses;
using UnityEngine;

namespace Radio
{
    public sealed class RadioButton : InteractiveObject
    {
        [SerializeField] private RadioManager.Action action;// действие кнопки

        private RadioManager mManager;
        private void OnEnable()
        {
            mManager = transform.parent.parent.GetComponent<RadioManager>();
        }
        public override void Interact(PlayerStatements pl)
        {
            mManager.SendMessage(action);         
        }
    }
}