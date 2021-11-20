using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Menu.Settings
{
    internal sealed class GameSubpanelSettings : SubpanelSettings
    {
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider genericVolumeSlider;
        [SerializeField] private Slider fieldOfViewVolumeSlider;
        [SerializeField] private TMP_Dropdown languageTMP_Dropdown;
        [SerializeField] private Toggle isDevModeToggle;

        protected override void OnInit()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFields()
        {
            throw new NotImplementedException();
        }
    }
}