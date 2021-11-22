using System;
using System.Collections;

using System.IO;
using System.Linq;
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
    /// ���� ��������, ����� ��� ���� ����.
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
            //��������� ��������� �� ����� � �����
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
        /// ������� ������ ��������
        /// </summary>
        internal void HidePanel()
        {
            gameObject.SetActive(false);

            menuManager.ShowGenericPanel();
        }

        /// <summary>
        /// ������� ������ ��������
        /// </summary>
        internal void ShowPanel()
        {
            gameObject.SetActive(true);

            ShowSubpanel(gameSubpanel);
        }


        private void SaveSettings()
        {
            ///�������� ��� ��������� ����������� ���� ������ <see cref="Society.Settings.GameSettings"/>            
            FieldInfo[] gameSettingsFI = typeof(Society.Settings.GameSettings).GetFields(BindingFlags.Static | BindingFlags.NonPublic);

            ///�������� ��� ��������� ����������� ���� ������ <see cref="Society.Settings.InputSettings"/>            
            FieldInfo[] inputSettingsFI = typeof(Society.Settings.InputSettings).GetFields(BindingFlags.Static | BindingFlags.NonPublic);

            ///�������� ��� ��������� ����������� ���� ������ <see cref="Society.Settings.VideoSettings"/>            
            FieldInfo[] videoSettingsFI = typeof(Society.Settings.VideoSettings).GetFields(BindingFlags.Static | BindingFlags.NonPublic);

            //������ ������������ ���������� ���� ���� ��������
            IEnumerable result = gameSettingsFI.Select(fieldGame => new { name = fieldGame.Name, value = fieldGame.GetValue(null) })
                     .Concat(inputSettingsFI.Select(fieldInput => new { name = fieldInput.Name, value = fieldInput.GetValue(null) })).
                     Concat(videoSettingsFI.Select(fieldVideo => new { name = fieldVideo.Name, value = fieldVideo.GetValue(null) }));

            //����������� ��� ���������
            string data = JsonConvert.SerializeObject(result, Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter());

            //���������� ��������� � ���� �� ����
            using (var stream = new FileStream(GetPathToSettings(),
                                                FileMode.OpenOrCreate,
                                                FileAccess.Write,
                                                FileShare.None))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(data);
                stream.Write(info, 0, info.Length);
            };
        }
        private void LoadSettings()
        {

            //try
            {
                var data = File.ReadAllText(GetPathToSettings());

                IEnumerable mergedAnonymeType = (IEnumerable)JsonConvert.DeserializeObject<object>(data);

                ///�������� ��� ��������� ����������� ���� ������ <see cref="Society.Settings.GameSettings"/>            
                FieldInfo[] gameSettingsFI = typeof(Society.Settings.GameSettings).GetFields(BindingFlags.Static | BindingFlags.NonPublic);

                foreach (var may in mergedAnonymeType)
                {
                    var anonymusObject = new { name, value = new object { } };
                    foreach (var field in gameSettingsFI)
                    {
                        var a = JsonConvert.DeserializeAnonymousType(may.ToString(), anonymusObject);

                        dynamic value = a.value;

                        if (a.name != field.Name)
                        {
                            continue;
                        }

                        if (field.FieldType.IsEnum)
                        {
                            //string tempEnum = (string)value;
                            value = Enum.Parse(field.FieldType, value);
                        }


                        field.SetValue(null, value);
                    }
                }
            }
            //catch
            {
                //  Debug.LogError("Failed load settings!");
            }
        }

        /// <summary>
        /// ���� ���������� ��������
        /// </summary>
        /// <returns></returns>
        private string GetPathToSettings() => Directory.GetCurrentDirectory() + "\\Saves\\Settings_V2.json";
    }
}