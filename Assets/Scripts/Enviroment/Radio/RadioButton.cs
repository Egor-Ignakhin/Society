using Society.Patterns;

using UnityEngine;

namespace Society.Enviroment.Radio
{
    sealed class RadioButton : InteractiveObject
    {
        [SerializeField] private RadioManager.Action action;// действие кнопки

        private RadioManager mManager;
        private void OnEnable()
        {
            mManager = transform.parent.parent.GetComponent<RadioManager>();
        }
        public override void Interact()
        {
            mManager.SendMessage(action);
        }
    }
}