
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

            SimplificationKeyCodesInitialization(moveFrontTMP_InputField, GameSettings.GetMoveFrontKeyCode());
            SimplificationKeyCodesInitialization(moveBackTMP_InputField, GameSettings.GetMoveBackKeyCode());
            SimplificationKeyCodesInitialization(moveLeftTMP_InputField, GameSettings.GetMoveLeftKeyCode());
            SimplificationKeyCodesInitialization(moveRightTMP_InputField, GameSettings.GetMoveRightKeyCode());
            SimplificationKeyCodesInitialization(leanLeftTMP_InputField, GameSettings.GetLeanLeftKeyCode());
            SimplificationKeyCodesInitialization(leanRightTMP_InputField, GameSettings.GetLeanRightKeyCode());
            SimplificationKeyCodesInitialization(jumpTMP_InputField, GameSettings.GetJumpKeyCode());
            SimplificationKeyCodesInitialization(crouchTMP_InputField, GameSettings.GetCrouchKeyCode());
            SimplificationKeyCodesInitialization(proneTMP_InputField, GameSettings.GetProneKeyCode());
            SimplificationKeyCodesInitialization(sprintTMP_InputField, GameSettings.GetSprintKeyCode());
            SimplificationKeyCodesInitialization(inventoryTMP_InputField, GameSettings.GetInventoryKeyCode());
            SimplificationKeyCodesInitialization(interactionTMP_InputField, GameSettings.GetInteractionKeyCode());
            SimplificationKeyCodesInitialization(reloadTMP_InputField, GameSettings.GetReloadKeyCode());

        }
        private void SimplificationKeyCodesInitialization(TMP_InputField tMP_InputField, KeyCode defaultKey)
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
            GameSettings.SetMouseSensivity(mouseSensivitySlider.value);

            GameSettings.SetMoveFrontKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), moveFrontTMP_InputField.text));
            GameSettings.SetMoveBackKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), moveBackTMP_InputField.text));
            GameSettings.SetMoveLeftKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), moveLeftTMP_InputField.text));
            GameSettings.SetMoveRightKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), moveRightTMP_InputField.text));
            GameSettings.SetLeanLeftKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), leanLeftTMP_InputField.text));
            GameSettings.SetLeanRightKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), leanRightTMP_InputField.text));            
            GameSettings.SetJumpKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), jumpTMP_InputField.text));
            GameSettings.SetCrouchKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), crouchTMP_InputField.text));
            GameSettings.SetProneKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), proneTMP_InputField.text));
            GameSettings.SetSprintKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), sprintTMP_InputField.text));
            GameSettings.SeInventoryKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), inventoryTMP_InputField.text));
            GameSettings.SetInteractionKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), interactionTMP_InputField.text));
            GameSettings.SetReloadKeyCode((KeyCode)System.Enum.Parse(typeof(KeyCode), reloadTMP_InputField.text));
        }

        protected override void UpdateFields()
        {
            mouseSensivitySlider.value = (float)GameSettings.GetMouseSensivity();
            mouseSensivityTMP.SetText(((int)mouseSensivitySlider.value).ToString());

            moveFrontTMP_InputField.SetTextWithoutNotify(GameSettings.GetMoveFrontKeyCode().ToString());
            moveBackTMP_InputField.SetTextWithoutNotify(GameSettings.GetMoveBackKeyCode().ToString());
            moveLeftTMP_InputField.SetTextWithoutNotify(GameSettings.GetMoveLeftKeyCode().ToString());
            moveRightTMP_InputField.SetTextWithoutNotify(GameSettings.GetMoveRightKeyCode().ToString());
            leanLeftTMP_InputField.SetTextWithoutNotify(GameSettings.GetLeanLeftKeyCode().ToString());
            leanRightTMP_InputField.SetTextWithoutNotify(GameSettings.GetLeanRightKeyCode().ToString());
            jumpTMP_InputField.SetTextWithoutNotify(GameSettings.GetJumpKeyCode().ToString());
            crouchTMP_InputField.SetTextWithoutNotify(GameSettings.GetCrouchKeyCode().ToString());
            proneTMP_InputField.SetTextWithoutNotify(GameSettings.GetProneKeyCode().ToString());
            sprintTMP_InputField.SetTextWithoutNotify(GameSettings.GetSprintKeyCode().ToString());
            inventoryTMP_InputField.SetTextWithoutNotify(GameSettings.GetInventoryKeyCode().ToString());
            interactionTMP_InputField.SetTextWithoutNotify(GameSettings.GetInteractionKeyCode().ToString());
            reloadTMP_InputField.SetTextWithoutNotify(GameSettings.GetReloadKeyCode().ToString());
        }
    }
}