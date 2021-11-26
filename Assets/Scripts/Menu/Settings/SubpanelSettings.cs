using Society.Settings;

using UnityEngine;

namespace Society.Menu.Settings
{
    /// <summary>
    /// Подпанель настроек
    /// </summary>
    public abstract class SubpanelSettings : MonoBehaviour
    {
        private void Awake()
        {
            GameSettings.SaveSettingsEvent += OnSettingsSave;

            OnInit();
        }
        private void OnEnable() => UpdateFields();

        protected abstract void UpdateFields();

        protected abstract void OnInit();

        protected abstract void OnSettingsSave();

        private void OnDestroy() => GameSettings.SaveSettingsEvent -= OnSettingsSave;
    }
}