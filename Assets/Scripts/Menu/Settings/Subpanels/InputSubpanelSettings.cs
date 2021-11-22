using System;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Menu.Settings
{
    internal sealed class InputSubpanelSettings : SubpanelSettings
    {
        [SerializeField] private Slider mouseSensivitySlider;
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
            throw new NotImplementedException();
        }

        protected override void UpdateFields()
        {
            throw new NotImplementedException();
        }

        protected override void OnSettingsSave()
        {
            throw new System.NotImplementedException();
        }
    }
}