
using TMPro;

using UniRx;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Menu.Settings
{
    internal sealed class GameSubpanelSettings : SubpanelSettings
    {
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider genericVolumeSlider;

        [Space(5)]
        [SerializeField] private Slider fieldOfViewVolumeSlider;
        [SerializeField] private TextMeshProUGUI fieldOfViewTMP;
        [Space(5)]

        [SerializeField] private TMP_Dropdown languageTMP_Dropdown;
        [SerializeField] private Toggle isDevModeToggle;

        protected override void OnInit()
        {
            fieldOfViewVolumeSlider.OnValueChangedAsObservable().Subscribe(_ => fieldOfViewTMP.SetText(((int)fieldOfViewVolumeSlider.value).ToString()));
        }

        protected override void OnSettingsSave()
        {
            Society.Settings.GameSettings.SetMusicVolume(musicVolumeSlider.value);
            Society.Settings.GameSettings.SetGeneralVolume(genericVolumeSlider.value);
            Society.Settings.GameSettings.SetFieldOfView(fieldOfViewVolumeSlider.value);
            Society.Settings.GameSettings.SetLanguage(languageTMP_Dropdown.value == 0 ? SystemLanguage.Russian : SystemLanguage.English);
            Society.Settings.GameSettings.SetIsDevMode(isDevModeToggle.isOn);
        }

        protected override void UpdateFields()
        {
            musicVolumeSlider.value = (float)Society.Settings.GameSettings.GetMusicVolume();
            genericVolumeSlider.value = (float)Society.Settings.GameSettings.GetGeneralVolume();

            fieldOfViewVolumeSlider.value = (float)Society.Settings.GameSettings.GetFieldOfView();
            fieldOfViewTMP.SetText(((int)fieldOfViewVolumeSlider.value).ToString());

            var systemLanguage = Society.Settings.GameSettings.GetSystemLanguage();
            languageTMP_Dropdown.value = (systemLanguage == SystemLanguage.Russian ? 0 : 1);
            isDevModeToggle.isOn = Society.Settings.GameSettings.GetIsDevMode();
        }
    }
}