using UnityEngine;

namespace Society.Menu.Settings
{
    public abstract class SubpanelSettings : MonoBehaviour
    {
        private void Awake()
        {
            SettingsManager.SaveSettingsEvent += OnSettingsSave;

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
            SettingsManager.SaveSettingsEvent -= OnSettingsSave;
        }
    }
}