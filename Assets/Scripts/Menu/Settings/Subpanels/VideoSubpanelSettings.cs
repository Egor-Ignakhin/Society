using System.Collections.Generic;

using TMPro;

using UniRx;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Menu.Settings
{
    internal sealed class VideoSubpanelSettings : SubpanelSettings
    {
        [SerializeField] private TMP_Dropdown graphicsQualityTMP_Dropdown;
        [SerializeField] private TMP_Dropdown resolutionQualityTMP_Dropdown;
        [SerializeField] private Toggle isFullScreenToggle;
        [SerializeField] private Toggle vSyncIsEnabledToggle;
        [SerializeField] private TMP_Dropdown antialiasingTypeTMP_Dropdown;
        [SerializeField] private Toggle bloomIsEnabledToggle;
        [SerializeField] private Toggle fogIsEnabledToggle;

        protected override void OnInit()
        {
            graphicsQualityTMP_Dropdown.FillOptionsWithNamesEnum(typeof(Society.Settings.GraphicsLevels));

            resolutionQualityTMP_Dropdown.FillOptionsWithNamesEnum(typeof(Society.Settings.ScreenResolutions));

            antialiasingTypeTMP_Dropdown.FillOptionsWithNamesEnum(
                typeof(UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData.AntialiasingMode));
        }


        protected override void OnSettingsSave()
        {
            Society.Settings.GameSettings.SetGraphicsQuality((Society.Settings.GraphicsLevels)graphicsQualityTMP_Dropdown.value);
            Society.Settings.GameSettings.SetScreenResolution((Society.Settings.ScreenResolutions)resolutionQualityTMP_Dropdown.value);
            Society.Settings.GameSettings.SetIsFullScreen(isFullScreenToggle.isOn);
            Society.Settings.GameSettings.SetVSyncIsEnabled(vSyncIsEnabledToggle.isOn);
            Society.Settings.GameSettings.SetAntialiasingType(
                (UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData.AntialiasingMode)antialiasingTypeTMP_Dropdown.value);
            Society.Settings.GameSettings.SetBloomIsEnabled(bloomIsEnabledToggle.isOn);
            Society.Settings.GameSettings.SetFogIsEnabled(fogIsEnabledToggle.isOn);
        }

        protected override void UpdateFields()
        {
            graphicsQualityTMP_Dropdown.value = (int)Society.Settings.GameSettings.GetQualityLevel();
            resolutionQualityTMP_Dropdown.value = (int)Society.Settings.GameSettings.GetScreenResolution();
            isFullScreenToggle.isOn = Society.Settings.GameSettings.GetIsFullScreen();
            vSyncIsEnabledToggle.isOn = Society.Settings.GameSettings.GetVSyncIsEnabled();
            bloomIsEnabledToggle.isOn = Society.Settings.GameSettings.GetBloomIsEnabled();
            fogIsEnabledToggle.isOn = Society.Settings.GameSettings.GetFogIsEnabled();
        }
    }
}