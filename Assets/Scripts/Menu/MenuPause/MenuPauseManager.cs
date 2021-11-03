using System.IO;

using Society.Effects;
using Society.GameScreens;
using Society.Menu.PauseMenu;
using Society.Player.Controllers;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Menu.PauseMenu
{
    /// <summary>
    /// класс - управлящий меню паузой
    /// </summary>
    internal sealed class MenuPauseManager : MonoBehaviour, IGameScreen
    {
        private MenuEventReceiver menuEventReceiver;// обработчик событий меню-паузы
        [SerializeField] private Transform mainParent;// контейнер сод. кнопки
        [SerializeField] private GameObject MenuUI;// главный бэкграунд и носитель кнопок
        [SerializeField] private GameObject SettingsObj;// меню настроек     
        [SerializeField] private Slider fovSlider;
        [SerializeField] private TextMeshProUGUI fovText;
        private FirstPersonController fpc;// контроллёр игрока
        private static CurrentGameSettings currentGameSettings;
        public static CurrentGameSettings GetCurrentGameSettings() => currentGameSettings;
        private readonly string pathForSettings = Directory.GetCurrentDirectory() + "\\Saves\\Settings.json";
        private EffectsManager effectsManager;
        [SerializeField] private Toggle bloomToggle;

        [SerializeField] private TextMeshProUGUI sensivityText;
        [SerializeField] private Slider sensivitySlider;
        [SerializeField] private Toggle reloadCABToggle;
        private void Awake()
        {
            fpc = FindObjectOfType<FirstPersonController>();
            effectsManager = FindObjectOfType<EffectsManager>();
            menuEventReceiver = new MenuEventReceiver(MenuUI, SettingsObj, this, effectsManager);
            LoadData();
        }
        private void Start()
        {

            fovSlider.value = (currentGameSettings.FOV - currentGameSettings.minFov) / (currentGameSettings.maxFov - currentGameSettings.minFov);
            fovText.SetText(currentGameSettings.FOV.ToString());
            Camera.main.fieldOfView = currentGameSettings.FOV;

            sensivitySlider.value = (float)currentGameSettings.Sensivity / 10;
            sensivityText.SetText(currentGameSettings.Sensivity.ToString());

            effectsManager.SetEnableBloom(currentGameSettings.BloomEnabled);
            effectsManager.SetEnableReloadCAB(currentGameSettings.reloadEffectEnabled);
            bloomToggle.isOn = currentGameSettings.BloomEnabled;
            reloadCABToggle.isOn = currentGameSettings.reloadEffectEnabled;
            SettingsObj.SetActive(false);
            sensivitySlider.onValueChanged.AddListener(ChangeSensivitySlider);
            fovSlider.onValueChanged.AddListener(ChangeFovSlider);

            SetSensivityToFPC();
        }

        public void Enable() => menuEventReceiver.Enable();

        private void SetSensivityToFPC()
        {
            fpc.SetSensivity(currentGameSettings.Sensivity);
        }

        /// <summary>
        /// смена активности bloom'а
        /// </summary>
        /// <param name="t"></param>
        public void SetActiveGlobalBloom()
        {
            if (effectsManager)
                effectsManager.SetEnableBloom(currentGameSettings.BloomEnabled = bloomToggle.isOn);
        }
        public void SetActiveReloadCAB()
        {
            if (effectsManager)
                effectsManager.SetEnableReloadCAB(currentGameSettings.reloadEffectEnabled = reloadCABToggle.isOn);
        }
        public void ChangeFovSlider(float v)
        {
            currentGameSettings.FOV = currentGameSettings.minFov + ((currentGameSettings.maxFov - currentGameSettings.minFov) * v);

            currentGameSettings.FOV = (float)System.Math.Round(currentGameSettings.FOV, 1);// округление до нормальных значений

            fovText.SetText(currentGameSettings.FOV.ToString());
            Camera.main.fieldOfView = currentGameSettings.FOV;
        }
        public void ChangeSensivitySlider(float v)
        {
            currentGameSettings.Sensivity = (int)(v * 10);

            sensivityText.SetText(currentGameSettings.Sensivity.ToString());
            SetSensivityToFPC();
        }
        private void LoadData()
        {
            try
            {
                string data = File.ReadAllText(pathForSettings);
                currentGameSettings = JsonUtility.FromJson<CurrentGameSettings>(data);
            }
            catch
            {
                currentGameSettings = new CurrentGameSettings();
            }
            if (currentGameSettings == null)
                currentGameSettings = new CurrentGameSettings();
        }
        private void OnDisable()
        {
            SaveData();
            sensivitySlider.onValueChanged.RemoveListener(ChangeSensivitySlider);
            fovSlider.onValueChanged.RemoveListener(ChangeFovSlider);
        }

        private void SaveData()
        {
            string data = JsonUtility.ToJson(currentGameSettings, true);
            File.WriteAllText(pathForSettings, data);
        }

        public bool Hide()
        {
            menuEventReceiver.Disable();
            return true;
        }
       

        public void BackToGame()
        {
            menuEventReceiver.Doing(CommandContainer.Doings.BakeToGame);            
        }
        public void OpenSettings()
        {
            menuEventReceiver.Doing(CommandContainer.Doings.OpenSettings);

        }
        public void GoToMainMenu()
        {
            menuEventReceiver.Doing(CommandContainer.Doings.GoToMainMenu);

        }

        public KeyCode HideKey() => KeyCode.Escape;

        sealed class AdvancedSettings
        {
            public static readonly Color SelectedColor = new Color(0.33f, 0.33f, 0.33f, 1);// цвет при наведении на кнопку
            public static readonly Color DefaultColor = new Color(0, 0, 0, 0);// обычный цвет кнопки
            public static readonly Color PressedColor = new Color(0.25f, 0.25f, 0.25f, 1);// цвет при нажатии на кнопку
        }
        private class MenuEventReceiver
        {
            private readonly GameObject menuUI;
            private readonly GameObject SettingsObj;
            private readonly CommandContainer commandContainer = new CommandContainer();            
            private readonly MenuPauseManager menuPauseManager;
            private readonly EffectsManager effectsManager;          
            public MenuEventReceiver(GameObject menu, GameObject stn, MenuPauseManager mpm, EffectsManager em)
            {
                menuUI = menu;
                SettingsObj = stn;                
                menuPauseManager = mpm;
                effectsManager = em;
                Disable();
            }

            public void Enable() => commandContainer.SetEnableMenu(true, menuUI, menuPauseManager, effectsManager);
            public void Disable()
            {
                commandContainer.SetEnableMenu(false, menuUI, menuPauseManager, effectsManager);
                SettingsObj.SetActive(false);
            }

            public void Doing(CommandContainer.Doings doi)
            {
                switch (doi)
                {
                    case CommandContainer.Doings.BakeToGame:
                        commandContainer.SetEnableMenu(false, menuUI, menuPauseManager, effectsManager);
                        break;
                    case CommandContainer.Doings.OpenSettings:
                        commandContainer.Settings(SettingsObj);
                        break;
                    case CommandContainer.Doings.GoToMainMenu:
                        commandContainer.ExitToMainMenu();
                        break;
                }
            }
        }
        public sealed class CommandContainer
        {
            public enum Doings
            {
                BakeToGame,
                OpenSettings,
                GoToMainMenu
            }
            public void Settings(GameObject SettingsObj)
            {
                SettingsObj.SetActive(!SettingsObj.activeInHierarchy);
            }
            public void ExitToMainMenu()
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesManager.MainMenu);
            }
            public void SetEnableMenu(bool v, GameObject menu, MenuPauseManager mpm, EffectsManager effectsManager)
            {
                menu.SetActive(v);
                Time.timeScale = v ? 0 : 1;
                // пауза при открытии инвентаря                                                        
                if (!v)
                {
                    ScreensManager.SetScreen(null);
                }
                else
                {
                    ScreensManager.SetScreen(mpm);
                }
                effectsManager.SetEnableSimpleDOF(v);
            }
        }
    }
    [System.Serializable]
    public class CurrentGameSettings
    {
        public float minFov = 60;
        public float FOV = 70;
        public float maxFov = 80;

        public bool BloomEnabled = true;

        public int MinSensivity = 0;
        public int Sensivity = 3;
        public int MaxSensivity = 10;

        public bool reloadEffectEnabled = true;
    }
}

internal class GameSettings
{
    public static float MinFov() => MenuPauseManager.GetCurrentGameSettings().minFov;
    public static float FOV() => MenuPauseManager.GetCurrentGameSettings().FOV;
    public static float MaxFov() => MenuPauseManager.GetCurrentGameSettings().maxFov;

    public static float GetSensivity() => MenuPauseManager.GetCurrentGameSettings().Sensivity;
}