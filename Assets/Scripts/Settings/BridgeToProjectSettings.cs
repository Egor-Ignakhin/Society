using Society.Settings;

using UnityEngine;

/// <summary>
/// "Мост" к проектным настройкам. 
/// Устанавливает нужные настройки, как только те обновляются.
/// </summary>
public class BridgeToProjectSettings
{
    public BridgeToProjectSettings() 
        => GameSettings.UpdateSettingsEvent += OnUpdateSettingsEvent;

    private void OnUpdateSettingsEvent()
    {
        //Устанавливаем галку "Полный экрна"
        Screen.fullScreen = GameSettings.GetIsFullScreen();

        //Устанавливаем ограничение частоты кадров
        QualitySettings.vSyncCount = GameSettings.GetVSyncIsEnabled() ? 1 : 0;

        //Устанавливаем уровень графики
        QualitySettings.SetQualityLevel(
            (int)System.Enum.Parse(typeof(GraphicsLevels), 
            GameSettings.GetQualityLevel().ToString()));

        //Устанавливаем разрешение экрана
        (int width, int height) = GameSettings.GetAndDescriptScreenResolution();
        Screen.SetResolution(width, height, GameSettings.GetIsFullScreen());
    }

    ~BridgeToProjectSettings()=>    
        GameSettings.UpdateSettingsEvent -= OnUpdateSettingsEvent;
    
}
