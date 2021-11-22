using UnityEngine;

namespace Society.Menu.Settings
{
    public abstract class SubpanelSettings : MonoBehaviour
    {
        private void Awake()
        {
            SettingsManager.ApplySettingsEvent += OnSettingsSave;

            OnInit();
        }
        private void OnEnable()
        {
            UpdateFields();
        }

        protected abstract void UpdateFields();

        protected abstract void OnInit();

        protected abstract void OnSettingsSave();

        private void OnDestroy()
        {
            SettingsManager.ApplySettingsEvent -= OnSettingsSave;
        }
    }
}