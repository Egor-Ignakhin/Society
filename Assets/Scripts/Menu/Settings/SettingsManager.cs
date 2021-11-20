using System;
using System.IO;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;

using Society.Patterns;

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

        public event Action ApplySettingsEvent;

        #endregion

        private void Awake()
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
                ApplySettingsEvent?.Invoke();

                SaveSettings();
            });

            HidePanel();
        }

        private void ShowSubpanel(GameObject subpanel)
        {
            gameSubpanel.SetActive(false);
            inputSubpanel.SetActive(false);
            videoSubpanel.SetActive(false);


            subpanel.SetActive(true);
        }

        /// <summary>
        /// Закрыть панель настроек
        /// </summary>
        internal void HidePanel()
        {
            gameObject.SetActive(false);

            menuManager.ShowGenericPanel();
        }

        /// <summary>
        /// Открыть панель настроек
        /// </summary>
        internal void ShowPanel()
        {
            gameObject.SetActive(true);

            ShowSubpanel(gameSubpanel);
        }


        private void SaveSettings()
        {
            ///Получаем все приватные статические поля класса <see cref="Society.Settings.GameSettings"/>            
            FieldInfo[] gameSettingsFI = typeof(Society.Settings.GameSettings).GetFields(BindingFlags.Static | BindingFlags.NonPublic);

            //Создаём анонимный тип содержащий поля всех настроек
            Type allSettings = new { Name = 1, Tytpe = "dsdslsdjkdfndjkfbdfdbkhdzfbkfdbhj" }.GetType();

            //Сериализуем все настройки
            string data = JsonConvert.SerializeObject(allSettings);

            string pathGameSettings = Directory.GetCurrentDirectory() + "\\Saves\\Settings_V2.json";

            //Записываем настройки в файл на диск
            using (var stream = new FileStream(pathGameSettings,
                                                FileMode.OpenOrCreate,
                                                FileAccess.ReadWrite,
                                                FileShare.None))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(data);
                stream.Write(info, 0, info.Length);
            };
        }        
    }
}