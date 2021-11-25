using System.Collections.Generic;

using Society.Effects;
using Society.GameScreens;
using Society.Menu.Settings;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Menu.MenuPause
{
    /// <summary>
    /// Управлящий меню паузой
    /// </summary>
    internal sealed class MenuPauseManager : MenuManager, IGameScreen
    {
        [SerializeField] private Transform mainParent;// контейнер сод. кнопки
        [SerializeField] private GameObject MenuUI;// главный бэкграунд и носитель кнопок

        /// <summary>
        /// Меню настроек
        /// </summary>
        [SerializeField] private SettingsManager settingsManager;

        private EffectsManager effectsManager;

        [SerializeField] private RawImage latestCameraRender;
        [SerializeField] private Camera menuCamera;
        private Camera mainCamera;

        private sealed class AdvancedSettings
        {
            public static readonly Color SelectedColor = new Color(0.33f, 0.33f, 0.33f, 1);// цвет при наведении на кнопку
            public static readonly Color DefaultColor = new Color(0, 0, 0, 0);// обычный цвет кнопки
            public static readonly Color PressedColor = new Color(0.25f, 0.25f, 0.25f, 1);// цвет при нажатии на кнопку
        }

        protected override void OnInit()
        {
            mainCamera = Camera.main;
            effectsManager = FindObjectOfType<EffectsManager>();

            Disable();
        }
        public void Enable() => SetEnableMenu(true);

        public bool Hide()
        {
            if (settingsManager.PanelIsActive())
            {
                settingsManager.HidePanel();
                return false;
            }

            Disable();
            return true;
        }

        public void BackToGame() => SetEnableMenu(false);
        public void OpenSettings()
        {
            genericPanel.SetActive(false);
            settingsManager.ShowPanel();
        }
        public void GoToMainMenu() => UnityEngine.SceneManagement.SceneManager.LoadScene((int)Scenes.MainMenu);

        public KeyCode HideKey() => KeyCode.Escape;

        private readonly List<Canvas> screenSpaceOverlayCanvases = new List<Canvas>();
        public void Disable()
        {
            SetEnableMenu(false);
            settingsManager.HidePanel();
        }

        private void SetEnableMenu(bool menuIsActive)
        {
            menuCamera.gameObject.SetActive(menuIsActive);
            MenuUI.SetActive(menuIsActive);
            Time.timeScale = menuIsActive ? 0 : 1;

            ScreensManager.SetScreen(menuIsActive ? this : null);

            effectsManager.SetEnableSimpleDOF(menuIsActive);

            SetupModeCanvases(menuIsActive);

            if (menuIsActive)
                latestCameraRender.texture = MakeScrenshot(mainCamera);

            mainCamera.enabled = !menuIsActive;

        }
        private void SetupModeCanvases(bool menuIsActive)
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
                    canvas.planeDistance = mainCamera.nearClipPlane * 1.1f;
                    canvas.worldCamera = mainCamera;
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