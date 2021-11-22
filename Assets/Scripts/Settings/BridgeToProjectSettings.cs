using Society.Settings;

using UnityEditor;

using UnityEngine;

public static class BridgeToProjectSettings
{
    static BridgeToProjectSettings()
    {
        Society.Menu.Settings.SettingsManager.SettingsUpdateEvent += OnUpdateSettingsEvent;
    }

    private static void OnUpdateSettingsEvent()
    {
        Screen.fullScreen = VideoSettings.GetIsFullScreen();

        QualitySettings.vSyncCount = VideoSettings.GetVSyncIsEnabled() ? 1 : 0;

        QualitySettings.SetQualityLevel((int)System.Enum.Parse(typeof(GraphicsLevels), VideoSettings.GetQualityLevel().ToString()));

        int screenWidth = 0;
        int screenHeight = 0;
        string[] splitedSR = VideoSettings.GetScreenResolution().ToString().Split('x');
        screenWidth = int.Parse(splitedSR[0].Substring(1, splitedSR.Length));
        screenHeight = int.Parse(splitedSR[1]);

        Screen.SetResolution(screenWidth, screenHeight, VideoSettings.GetIsFullScreen());

    }
}
