using UnityEngine;

namespace Society.Menu.Settings
{
    public abstract class SubpanelSettings : MonoBehaviour
    {
        private void Awake()
        {
            SettingsManager.Instance.ApplySettingsEvent += OnSettingsSave;

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
            if(SettingsManager.Instance != null) 
                SettingsManager.Instance.ApplySettingsEvent -= OnSettingsSave;
        }
    }
}