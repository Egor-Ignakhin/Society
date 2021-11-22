
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
            //         musicVolumeSlider.OnValueChangedAsObservable().Subscribe(_ => Society.Settings.GameSettings.S);
        }

        protected override void OnSettingsSave()
        {
            Society.Settings.GameSettings.SetMusicVolume(musicVolumeSlider.value);
        }

        protected override void UpdateFields()
        {
            musicVolumeSlider.SetValueWithoutNotify((float)Society.Settings.GameSettings.GetMusicVolume());
        }
    }
}