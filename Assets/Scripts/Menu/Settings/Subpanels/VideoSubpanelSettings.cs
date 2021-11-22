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
        [SerializeField] private Toggle bloomIsEnabledToggle;
        [SerializeField] private Toggle fogIsEnabledToggle;

        protected override void OnInit()
        {
            List<string> graphicsQualities = new List<string>();

            for (int i = 0; i < System.Enum.GetNames(typeof(Society.Settings.GraphicsLevels)).Length; i++)
                graphicsQualities.Add(System.Enum.GetNames(typeof(Society.Settings.GraphicsLevels))[i].ToString());

            graphicsQualityTMP_Dropdown.AddOptions(graphicsQualities);

            List<string> screenResolutions = new List<string>();

            for (int i = 0; i < System.Enum.GetNames(typeof(Society.Settings.ScreenResolutions)).Length; i++)
                screenResolutions.Add(System.Enum.GetNames(typeof(Society.Settings.ScreenResolutions))[i].ToString());

            resolutionQualityTMP_Dropdown.AddOptions(screenResolutions);
        }


        protected override void OnSettingsSave()
        {
            Society.Settings.VideoSettings.SetGraphicsQuality((Society.Settings.GraphicsLevels)graphicsQualityTMP_Dropdown.value);
            Society.Settings.VideoSettings.SetScreenResolution((Society.Settings.ScreenResolutions)resolutionQualityTMP_Dropdown.value);
            Society.Settings.VideoSettings.SetIsFullScreen(isFullScreenToggle.isOn);
            Society.Settings.VideoSettings.SetVSyncIsEnabled(vSyncIsEnabledToggle.isOn);
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