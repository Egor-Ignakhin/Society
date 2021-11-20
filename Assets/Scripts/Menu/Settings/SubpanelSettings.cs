using UnityEngine;

namespace Society.Menu.Settings
{
    public abstract class SubpanelSettings : MonoBehaviour
    {
        private void Awake()
        {
            OnInit();
        }
        private void OnEnable()
        {
            UpdateFields();
        }

        protected abstract void UpdateFields();

        protected abstract void OnInit();
    }
}