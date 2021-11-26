using System.Collections.Generic;

using Society.Effects;
using Society.GameScreens;
using Society.Menu.Settings;

using UniRx;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Menu.MenuPause
{
    #region Properties
    /// <summary>
    /// Управлящий меню паузой в обычной игре.
    /// </summary>
    internal sealed class MenuPauseManager : MenuManager, IGameScreen
    {
        /// <summary>
        /// Меню
        /// </summary>
        [SerializeField] private GameObject MenuUI;

        /// <summary>
        /// Панель настроек
        /// </summary>
        [SerializeField] private SettingsManager settingsManager;

        /// <summary>
        /// Управляющий эффектами сцены
        /// </summary>
        private EffectsManager effectsManager;

        /// <summary>
        /// Фоновое изображение. 
        /// Создаётся каждый раз при запуске меню.
        /// </summary>
        [SerializeField] private RawImage latestCameraRender;

        /// <summary>
        /// Камера отрисовывающая меню паузы.
        /// </summary>
        [SerializeField] private Camera menuCamera;

        /// <summary>
        /// Камера игрока
        /// </summary>
        private Camera playerCamera;

        [SerializeField] private Button backToGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitToMainMenuButton;

        private readonly List<Canvas> screenSpaceOverlayCanvases = new List<Canvas>();

        public KeyCode HideKey() => KeyCode.Escape;

        #endregion

        /// <summary>
        /// Инициализация меню
        /// </summary>
        protected override void OnInit()
        {
            playerCamera = Camera.main;
            effectsManager = FindObjectOfType<EffectsManager>();

            backToGameButton.OnClickAsObservable().Subscribe(_ => SetEnableMenu(false));
            settingsButton.OnClickAsObservable().Subscribe(_ =>
            {
                genericPanel.SetActive(false);
                settingsManager.ShowPanel();
            });
            exitToMainMenuButton.OnClickAsObservable().Subscribe(_ => UnityEngine.SceneManagement.SceneManager.LoadScene((int)Scenes.MainMenu));

            //Выключение меню после инициализации
            DisableMenu();
        }

        /// <summary>
        /// Включение меню
        /// </summary>
        public void EnableMenu() => SetEnableMenu(true);

        /// <summary>
        /// Скрыть меню.
        /// </summary>
        /// <returns></returns>
        public bool Hide()
        {
            //Если меню настроек активно
            if (settingsManager.PanelIsActive())
            {
                //Меню настроек выключается
                settingsManager.HidePanel();

                return false;
            }

            //Меню выключается
            DisableMenu();

            return true;
        }        

        /// <summary>
        /// Выключение меню
        /// </summary>
        public void DisableMenu()
        {
            SetEnableMenu(false);
            settingsManager.HidePanel();
        }

        /// <summary>
        /// Включение меню
        /// </summary>
        /// <param name="menuIsActive"></param>
        private void SetEnableMenu(bool menuIsActive)
        {
            menuCamera.gameObject.SetActive(menuIsActive);
            MenuUI.SetActive(menuIsActive);
            Time.timeScale = menuIsActive ? 0 : 1;

            ScreensManager.SetScreen(menuIsActive ? this : null);

            effectsManager.SetEnableSimpleDOF(menuIsActive);

            PrepareCanvasesModeToScreenShot(menuIsActive);

            if (menuIsActive)
                latestCameraRender.texture = MakeScrenshot(playerCamera);

            playerCamera.enabled = !menuIsActive;

        }

        /// <summary>
        /// Подготовка с созданию скриншота
        /// </summary>
        /// <param name="menuIsActive"></param>
        private void PrepareCanvasesModeToScreenShot(bool menuIsActive)
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
                    canvas.planeDistance = playerCamera.nearClipPlane * 1.1f;
                    canvas.worldCamera = playerCamera;
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