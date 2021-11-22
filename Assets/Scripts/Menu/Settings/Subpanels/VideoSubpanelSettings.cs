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
            Society.Settings.VideoSettings.SetGraphicsQuality((Society.Settings.GraphicsLevels)graphicsQualityTMP_Dropdown.value);
            Society.Settings.VideoSettings.SetScreenResolution((Society.Settings.ScreenResolutions)resolutionQualityTMP_Dropdown.value);
            Society.Settings.VideoSettings.SetIsFullScreen(isFullScreenToggle.isOn);
            Society.Settings.VideoSettings.SetVSyncIsEnabled(vSyncIsEnabledToggle.isOn);
            Society.Settings.VideoSettings.SetAntialiasingType(
                (UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData.AntialiasingMode)antialiasingTypeTMP_Dropdown.value);
            Society.Settings.VideoSettings.SetBloomIsEnabled(bloomIsEnabledToggle.isOn);
            Society.Settings.VideoSettings.SetFogIsEnabled(fogIsEnabledToggle.isOn);
        }

        protected override void UpdateFields()
        {
            graphicsQualityTMP_Dropdown.value = (int)Society.Settings.VideoSettings.GetQualityLevel();
            resolutionQualityTMP_Dropdown.value = (int)Society.Settings.VideoSettings.GetScreenResolution();
            isFullScreenToggle.isOn = Society.Settings.VideoSettings.GetIsFullScreen();
            vSyncIsEnabledToggle.isOn = Society.Settings.VideoSettings.GetVSyncIsEnabled();
            bloomIsEnabledToggle.isOn = Society.Settings.VideoSettings.GetBloomIsEnabled();
            fogIsEnabledToggle.isOn = Society.Settings.VideoSettings.GetFogIsEnabled();
        }
    }
}