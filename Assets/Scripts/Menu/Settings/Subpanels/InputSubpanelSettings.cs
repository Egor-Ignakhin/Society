
using Society.Settings;

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

        [SerializeField] private TMP_InputField moveFrontTMP_InputField;
        [SerializeField] private TMP_InputField moveBackTMP_InputField;
        [SerializeField] private TMP_InputField moveLeftTMP_InputField;
        [SerializeField] private TMP_InputField moveRightTMP_InputField;
        [SerializeField] private TMP_InputField leanLeftTMP_InputField;
        [SerializeField] private TMP_InputField leanRightTMP_InputField;
        [SerializeField] private TMP_InputField jumpTMP_InputField;
        [SerializeField] private TMP_InputField crouchTMP_InputField;
        [SerializeField] private TMP_InputField proneTMP_InputField;
        [SerializeField] private TMP_InputField sprintTMP_InputField;
        [SerializeField] private TMP_InputField inventoryTMP_InputField;
        [SerializeField] private TMP_InputField interactionTMP_InputField;
        [SerializeField] private TMP_InputField reloadTMP_InputField;

        protected override void OnInit()
        {
            mouseSensivitySlider.OnValueChangedAsObservable().Subscribe(_ => mouseSensivityTMP.SetText(((int)mouseSensivitySlider.value).ToString()));

            SimplificationKeyCodesInit(moveFrontTMP_InputField, InputSettings.GetMoveFrontKeyCode());
            SimplificationKeyCodesInit(moveBackTMP_InputField, InputSettings.GetMoveBackKeyCode());
            SimplificationKeyCodesInit(moveLeftTMP_InputField, InputSettings.GetMoveLeftKeyCode());
            SimplificationKeyCodesInit(moveRightTMP_InputField, InputSettings.GetMoveRightKeyCode());
            SimplificationKeyCodesInit(leanLeftTMP_InputField, InputSettings.GetLeanLeftKeyCode());
            SimplificationKeyCodesInit(leanRightTMP_InputField, InputSettings.GetLeanRightKeyCode());
            SimplificationKeyCodesInit(jumpTMP_InputField, InputSettings.GetJumpKeyCode());
            SimplificationKeyCodesInit(crouchTMP_InputField, InputSettings.GetCrouchKeyCode());
            SimplificationKeyCodesInit(proneTMP_InputField, InputSettings.GetProneKeyCode());
            SimplificationKeyCodesInit(sprintTMP_InputField, InputSettings.GetSprintKeyCode());
            SimplificationKeyCodesInit(inventoryTMP_InputField, InputSettings.GetInventoryKeyCode());
            SimplificationKeyCodesInit(interactionTMP_InputField, InputSettings.GetInteractionKeyCode());
            SimplificationKeyCodesInit(reloadTMP_InputField, InputSettings.GetReloadKeyCode());

        }
        private void SimplificationKeyCodesInit(TMP_InputField tMP_InputField, KeyCode defaultKey)
        {
            tMP_InputField.onValueChanged.AsObservable().Subscribe(_ =>
            {
                tMP_InputField.SetTextWithoutNotify(tMP_InputField.text.ToUpper());
            });
            tMP_InputField.onDeselect.AsObservable().Subscribe(_ =>
            {
                if (tMP_InputField.text.Length == 0)
                    tMP_InputField.SetTextWithoutNotify(defaultKey.ToString());
            });
        }


        protected override void OnSettingsSave()
        {
            InputSettings.SetMouseSensivity(mouseSensivitySlider.value);

            InputSettings.SetMoveFrontKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), moveFrontTMP_InputField.text));
            InputSettings.SetMoveBackKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), moveBackTMP_InputField.text));
            InputSettings.SetMoveLeftKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), moveLeftTMP_InputField.text));
            InputSettings.SetMoveRightKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), moveRightTMP_InputField.text));
            InputSettings.SetLeanLeftKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), leanLeftTMP_InputField.text));
            InputSettings.SetLeanRightKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), leanRightTMP_InputField.text)); InputSettings.SetMoveFrontKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), moveFrontTMP_InputField.text));
            InputSettings.SetJumpKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), jumpTMP_InputField.text));
            InputSettings.SetCrouchKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), crouchTMP_InputField.text));
            InputSettings.SetProneKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), proneTMP_InputField.text));
            InputSettings.SetSprintKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), sprintTMP_InputField.text));
            InputSettings.SeInventoryKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), inventoryTMP_InputField.text));
            InputSettings.SetInteractionKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), interactionTMP_InputField.text));
            InputSettings.SetReloadKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), reloadTMP_InputField.text));
        }

        protected override void UpdateFields()
        {
            mouseSensivitySlider.value = (float)InputSettings.GetMouseSensivity();
            mouseSensivityTMP.SetText(((int)mouseSensivitySlider.value).ToString());

            moveFrontTMP_InputField.SetTextWithoutNotify(InputSettings.GetMoveFrontKeyCode().ToString());
            moveBackTMP_InputField.SetTextWithoutNotify(InputSettings.GetMoveBackKeyCode().ToString());
            moveLeftTMP_InputField.SetTextWithoutNotify(InputSettings.GetMoveLeftKeyCode().ToString());
            moveRightTMP_InputField.SetTextWithoutNotify(InputSettings.GetMoveRightKeyCode().ToString());
            leanLeftTMP_InputField.SetTextWithoutNotify(InputSettings.GetLeanLeftKeyCode().ToString());
            leanRightTMP_InputField.SetTextWithoutNotify(InputSettings.GetLeanRightKeyCode().ToString());
            jumpTMP_InputField.SetTextWithoutNotify(InputSettings.GetJumpKeyCode().ToString());
            crouchTMP_InputField.SetTextWithoutNotify(InputSettings.GetCrouchKeyCode().ToString());
            proneTMP_InputField.SetTextWithoutNotify(InputSettings.GetProneKeyCode().ToString());
            sprintTMP_InputField.SetTextWithoutNotify(InputSettings.GetSprintKeyCode().ToString());
            inventoryTMP_InputField.SetTextWithoutNotify(InputSettings.GetInventoryKeyCode().ToString());
            interactionTMP_InputField.SetTextWithoutNotify(InputSettings.GetInteractionKeyCode().ToString());
            reloadTMP_InputField.SetTextWithoutNotify(InputSettings.GetReloadKeyCode().ToString());
        }
    }
}