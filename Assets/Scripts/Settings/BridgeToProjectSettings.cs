using Society.Settings;

using UnityEngine;

public class BridgeToProjectSettings
{
    public BridgeToProjectSettings()
    {
        Society.Menu.Settings.SettingsManager.SettingsUpdateEvent += OnUpdateSettingsEvent;
    }

    private void OnUpdateSettingsEvent()
    {
        Screen.fullScreen = GameSettings.GetIsFullScreen();

        QualitySettings.vSyncCount = GameSettings.GetVSyncIsEnabled() ? 1 : 0;

        QualitySettings.SetQualityLevel((int)System.Enum.Parse(typeof(GraphicsLevels), GameSettings.GetQualityLevel().ToString()));

        var wh = GameSettings.GetAndDescriptScreenResolution();

        Screen.SetResolution(wh.width, wh.height, GameSettings.GetIsFullScreen());
    }

    ~BridgeToProjectSettings()
    {
        Society.Menu.Settings.SettingsManager.SettingsUpdateEvent -= OnUpdateSettingsEvent;
    }
}
