using UnityEngine;

namespace Society.Player
{
    internal sealed class PlayerCamera : MonoBehaviour
    {
        private Camera playerCamera;
        private void Awake()
        {
            playerCamera = Camera.main;

            Society.Menu.Settings.SettingsManager.SettingsUpdateEvent += OnUpdateSettings;
        }

        private void OnUpdateSettings()
        {
            playerCamera.fieldOfView = (float)Settings.GameSettings.GetFieldOfView();
        }
        private void OnDestroy()
        {
            Society.Menu.Settings.SettingsManager.SettingsUpdateEvent -= OnUpdateSettings;
        }
    }
}