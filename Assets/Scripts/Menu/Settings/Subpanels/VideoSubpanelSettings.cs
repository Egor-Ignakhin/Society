using TMPro;

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
            throw new System.NotImplementedException();
        }

        protected override void UpdateFields()
        {
            throw new System.NotImplementedException();
        }
    }
}