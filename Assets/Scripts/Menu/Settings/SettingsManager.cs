using System;
using System.Collections;

using System.IO;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;

using Society.Patterns;
using Society.Settings;

using UniRx;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Menu.Settings
{
    /// <summary>
    /// Меню настроек, едино для всех меню.
    /// </summary>
    public sealed class SettingsManager : Singleton<SettingsManager>
    {
        #region Properties

        #region Buttons_supanels

        [SerializeField] private Button buttonGame;
        [SerializeField] private Button buttonInput;
        [SerializeField] private Button buttonVideo;

        #endregion

        [Space(5)]

        #region GenericButtons

        [SerializeField] private Button backButton;
        [SerializeField] private Button applyButton;

        #endregion

        [Space(5)]

        #region SubPanels

        [SerializeField] private GameObject gameSubpanel;
        [SerializeField] private GameObject inputSubpanel;
        [SerializeField] private GameObject videoSubpanel;

        #endregion

        [Space(5)]

        [SerializeField] private MenuManager menuManager;

        public static event Action SettingsUpdateEvent;

        public static event Action SaveSettingsEvent;


        private BridgeToProjectSettings bridgeToProjectSettings;

        [SerializeField] private GameObject settingsPanel;

        #endregion

        public void Awake()
        {
            buttonGame.OnClickAsObservable().Subscribe(_ => ShowSubpanel(gameSubpanel));
            buttonInput.OnClickAsObservable().Subscribe(_ => ShowSubpanel(inputSubpanel));
            buttonVideo.OnClickAsObservable().Subscribe(_ => ShowSubpanel(videoSubpanel));


            backButton.OnClickAsObservable().Subscribe(_ =>
            {
                HidePanel();
            });
            applyButton.OnClickAsObservable().Subscribe(_ =>
            {
                SaveSettingsEvent?.Invoke();

                SaveSettings();

                SettingsUpdateEvent?.Invoke();
            });

            bridgeToProjectSettings = new BridgeToProjectSettings();            
        }

        private void Start()
        {
            SettingsUpdateEvent?.Invoke();
        }

        private void ShowSubpanel(GameObject subpanel)
        {
            gameSubpanel.SetActive(false);
            inputSubpanel.SetActive(false);
            videoSubpanel.SetActive(false);


            subpanel.SetActive(true);

            SettingsUpdateEvent?.Invoke();
        }

        /// <summary>
        /// Закрыть панель настроек
        /// </summary>
        internal void HidePanel()
        {
            settingsPanel.SetActive(false);

            menuManager.ShowGenericPanel();
        }

        /// <summary>
        /// Открыть панель настроек
        /// </summary>
        internal void ShowPanel()
        {
            settingsPanel.SetActive(true);

            ShowSubpanel(gameSubpanel);
        }

        internal bool PanelIsActive()
        {
            return settingsPanel.activeInHierarchy;
        }

        private void SaveSettings()
        {
            //Сериализуем все настройки
            string data = JsonConvert.SerializeObject(GameSettings.GetSerializableSettins(),
                Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter(), new Newtonsoft.Json.Converters.BoolToStringConverter());

            //Записываем настройки в файл на диск
            File.WriteAllText(GameSettings.GetPathToSettings(), data);
        }
    }
}