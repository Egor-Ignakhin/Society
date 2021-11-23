using System;
using System.Collections;

using System.IO;
using System.Linq;
using System.Reflection;

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

        public static event Action SettingsUpdateEvent;

        public static event Action ApplySettingsEvent;

        #endregion

        private void Awake()
        {
            //Загружаем настройки из файла с диска
            LoadSettings();

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

                SettingsUpdateEvent?.Invoke();
            });
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

            ///Получаем все приватные статические поля класса <see cref="Society.Settings.InputSettings"/>            
            FieldInfo[] inputSettingsFI = typeof(Society.Settings.GameSettings).GetFields(BindingFlags.Static | BindingFlags.NonPublic);

            ///Получаем все приватные статические поля класса <see cref="Society.Settings.VideoSettings"/>            
            FieldInfo[] videoSettingsFI = typeof(Society.Settings.GameSettings).GetFields(BindingFlags.Static | BindingFlags.NonPublic);

            //Создаём перечисление содержащее поля всех настроек
            IEnumerable result = gameSettingsFI.Select(fieldGame => new { name = fieldGame.Name, value = fieldGame.GetValue(null) })
                     .Concat(inputSettingsFI.Select(fieldInput => new { name = fieldInput.Name, value = fieldInput.GetValue(null) })).
                     Concat(videoSettingsFI.Select(fieldVideo => new { name = fieldVideo.Name, value = fieldVideo.GetValue(null) }));

            //Сериализуем все настройки
            string data = JsonConvert.SerializeObject(Society.Settings.GameSettings.GetSerializableSettins()
                , Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter(), new Newtonsoft.Json.Converters.BoolToStringConverter());

            //Записываем настройки в файл на диск
            File.WriteAllText(GetPathToSettings(), data);

            print("Save");
        }
        private void LoadSettings()
        {

            var data = File.ReadAllText(GetPathToSettings());

            //  IEnumerable mergedAnonymeType = (IEnumerable)JsonConvert.DeserializeObject<object>(data);

            ///Получаем все приватные статические поля класса <see cref="Society.Settings.GameSettings"/>            
            //  FieldInfo[] gameSettingsFI = typeof(Society.Settings.GameSettings).GetFields(BindingFlags.Static | BindingFlags.NonPublic);

            //foreach (var may in mergedAnonymeType)
            // {
            // var anonymusObject = new { name, value = new object { } };
            // foreach (var field in gameSettingsFI)
            // {
            Society.Settings.GameSettings.SetSerializableSettins(JsonConvert.DeserializeObject<Society.Settings.GameSettings.SerializableGameSettins>(data));

            /*
            if (a.name != field.Name)
            {
                continue;
            }

            if (field.FieldType.IsEnum)
            {
                value = Enum.Parse(field.FieldType, value);
            }


            if ((field.FieldType == typeof(bool))
                && ((value == "false") || (value == "true")))
            {
                if (value == "false")
                    value = false;
                if (value == "true")
                    value = true;
            }

            field.SetValue(null, value);*/
            // }
            // }
        }

        /// <summary>
        /// Путь сохранения настроек
        /// </summary>
        /// <returns></returns>
        private string GetPathToSettings() => Directory.GetCurrentDirectory() + "\\Saves\\Settings.json";
    }
}