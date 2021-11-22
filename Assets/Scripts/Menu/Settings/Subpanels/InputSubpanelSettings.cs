using System;

using TMPro;

using UniRx;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Menu.Settings
{
    internal sealed class InputSubpanelSettings : SubpanelSettings
    {
        [Space(5)]
        [SerializeField] private Slider mouseSensivitySlider;
        [SerializeField] private TextMeshProUGUI mouseSensivityTMP;
        [Space(5)]

        [SerializeField] private Button moveFrontButton;
        [SerializeField] private Button moveBackButton;
        [SerializeField] private Button moveLeftButton;
        [SerializeField] private Button moveRightButton;
        [SerializeField] private Button leanLeftButton;
        [SerializeField] private Button leanRightButton;
        [SerializeField] private Button jumpButton;
        [SerializeField] private Button crouchButton;
        [SerializeField] private Button proneButton;
        [SerializeField] private Button sprintButton;
        [SerializeField] private Button inventoryButton;
        [SerializeField] private Button interactionButton;
        [SerializeField] private Button reloadButton;

        protected override void OnInit()
        {
            mouseSensivitySlider.OnValueChangedAsObservable().Subscribe(_ => mouseSensivityTMP.SetText(((int)mouseSensivitySlider.value).ToString()));
        }
        

        protected override void OnSettingsSave()
        {
            Society.Settings.InputSettings.SetMouseSensivity(mouseSensivitySlider.value);
        }

        protected override void UpdateFields()
        {            
            mouseSensivitySlider.value = (float)Society.Settings.InputSettings.GetMouseSensivity();
            mouseSensivityTMP.SetText(((int)mouseSensivitySlider.value).ToString());
        }
    }
}