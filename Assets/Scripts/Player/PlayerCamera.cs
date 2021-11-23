using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Society.Player
{
    internal sealed class PlayerCamera : MonoBehaviour
    {
        private Camera myCamera;
        private HDAdditionalCameraData additionalCameraData;
        private void Awake()
        {
            myCamera = Camera.main;
            additionalCameraData = GetComponent<HDAdditionalCameraData>();

            Society.Menu.Settings.SettingsManager.SettingsUpdateEvent += OnUpdateSettings;
        }

        private void OnUpdateSettings()
        {
            myCamera.fieldOfView = (float)Settings.GameSettings.GetFieldOfView();
            additionalCameraData.antialiasing = Settings.GameSettings.GetAntialiasingType();
        }
        private void OnDestroy()
        {
            Society.Menu.Settings.SettingsManager.SettingsUpdateEvent -= OnUpdateSettings;
        }
    }
}