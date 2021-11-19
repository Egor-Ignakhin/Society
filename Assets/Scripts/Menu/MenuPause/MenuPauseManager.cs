using System.Collections.Generic;
using System.IO;

using Society.Effects;
using Society.GameScreens;
using Society.Menu.Settings;
using Society.Player.Controllers;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Menu.MenuPause
{
    /// <summary>
    /// класс - управлящий меню паузой
    /// </summary>
    internal sealed class MenuPauseManager : MenuManager, IGameScreen
    {
        private MenuEventReceiver menuEventReceiver;// обработчик событий меню-паузы
        [SerializeField] private Transform mainParent;// контейнер сод. кнопки
        [SerializeField] private GameObject MenuUI;// главный бэкграунд и носитель кнопок

        /// <summary>
        /// Меню настроек
        /// </summary>
        [SerializeField] private SettingsManager settingsManager;

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
        [SerializeField] private RawImage latestCameraRender;
        [SerializeField] private Camera menuCamera;
        private void Awake()
        {
            fpc = FindObjectOfType<FirstPersonController>();
            effectsManager = FindObjectOfType<EffectsManager>();
            menuEventReceiver = new MenuEventReceiver(MenuUI, settingsManager, this, effectsManager, latestCameraRender, menuCamera);
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
            bloomToggle.isOn = currentGameSettings.BloomEnabled;
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
            genericPanel.SetActive(false);
            menuEventReceiver.Doing(CommandContainer.Doings.OpenSettings);

        }
        public void GoToMainMenu()
        {
            menuEventReceiver.Doing(CommandContainer.Doings.GoToMainMenu);

        }

        public KeyCode HideKey() => KeyCode.Escape;

        private sealed class AdvancedSettings
        {
            public static readonly Color SelectedColor = new Color(0.33f, 0.33f, 0.33f, 1);// цвет при наведении на кнопку
            public static readonly Color DefaultColor = new Color(0, 0, 0, 0);// обычный цвет кнопки
            public static readonly Color PressedColor = new Color(0.25f, 0.25f, 0.25f, 1);// цвет при нажатии на кнопку
        }
        private class MenuEventReceiver
        {
            private readonly GameObject menuUI;
            private readonly SettingsManager settingsManager;
            private readonly CommandContainer commandContainer;
            private readonly MenuPauseManager menuPauseManager;
            private readonly EffectsManager effectsManager;
            private readonly Camera menuCamera;
            public MenuEventReceiver(GameObject menu, SettingsManager settignsManager, MenuPauseManager mpm, EffectsManager em, RawImage latestCameraRender, Camera menuCamera)
            {
                menuUI = menu;
                settingsManager = settignsManager;
                menuPauseManager = mpm;
                effectsManager = em;
                this.menuCamera = menuCamera;
                commandContainer = new CommandContainer(Camera.main, latestCameraRender);

                Disable();
            }

            public void Enable() => commandContainer.SetEnableMenu(true, menuUI, menuPauseManager, effectsManager, menuCamera);
            public void Disable()
            {
                commandContainer.SetEnableMenu(false, menuUI, menuPauseManager, effectsManager, menuCamera);
                settingsManager.HidePanel();
            }

            public void Doing(CommandContainer.Doings doi)
            {
                switch (doi)
                {
                    case CommandContainer.Doings.BakeToGame:
                        commandContainer.SetEnableMenu(false, menuUI, menuPauseManager, effectsManager, menuCamera);
                        break;
                    case CommandContainer.Doings.OpenSettings:
                        settingsManager.ShowPanel();
                        break;
                    case CommandContainer.Doings.GoToMainMenu:
                        commandContainer.ExitToMainMenu();
                        break;
                }
            }
        }
        public sealed class CommandContainer
        {
            private readonly Camera mainCamera;
            private readonly RawImage latestCameraRender;
            private List<Canvas> screenSpaceOverlayCanvases = new List<Canvas>();

            public CommandContainer(Camera mainCamera, RawImage latestCameraRender)
            {
                this.mainCamera = mainCamera;
                this.latestCameraRender = latestCameraRender;
            }
            public enum Doings
            {
                BakeToGame,
                OpenSettings,
                GoToMainMenu
            }
            public void ExitToMainMenu()
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesManager.MainMenu);
            }
            public void SetEnableMenu(bool menuIsActive, GameObject menu, MenuPauseManager mpm, EffectsManager effectsManager, Camera menuCamera)
            {
                menuCamera.gameObject.SetActive(menuIsActive);
                menu.SetActive(menuIsActive);
                Time.timeScale = menuIsActive ? 0 : 1;

                ScreensManager.SetScreen(menuIsActive ? mpm : null);

                effectsManager.SetEnableSimpleDOF(menuIsActive);

                SetupModeCanvases(menuIsActive, mainCamera);

                if (menuIsActive)
                    latestCameraRender.texture = MakeScrenshot(mainCamera);                

                mainCamera.enabled = !menuIsActive;

            }
            private void SetupModeCanvases(bool menuIsActive, Camera camera)
            {
                if (menuIsActive)
                {
                    foreach (var c in FindObjectsOfType<Canvas>())
                    {
                        if (c.renderMode == RenderMode.ScreenSpaceOverlay)
                            screenSpaceOverlayCanvases.Add(c);
                    }
                }

                foreach (Canvas canvas in screenSpaceOverlayCanvases)
                {
                    if (!menuIsActive)
                    {
                        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                        canvas.worldCamera = null;
                    }
                    else
                    {
                        canvas.renderMode = RenderMode.ScreenSpaceCamera;
                        canvas.planeDistance = camera.nearClipPlane * 1.1f;
                        canvas.worldCamera = camera;
                    }
                }
            }
            public Texture2D MakeScrenshot(Camera camera)
            {
                int width = camera.pixelWidth;
                int height = camera.pixelHeight;
                Texture2D texture = new Texture2D(width, height);

                RenderTexture targetTexture = RenderTexture.GetTemporary(width, height);

                var latestTargetT = camera.targetTexture;
                camera.targetTexture = targetTexture;
                camera.Render();
                camera.targetTexture = latestTargetT;


                RenderTexture.active = targetTexture;

                Rect rect = new Rect(0, 0, width, height);
                texture.ReadPixels(rect, 0, 0);
                texture.Apply();

                return texture;
            }
        }
    }    
}