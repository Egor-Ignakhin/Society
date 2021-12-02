using System.IO;

using Newtonsoft.Json;

using Society.GameScreens;
using Society.Menu.Settings;

using UniRx;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Society.Menu
{
    /// <summary>
    /// Главное меню игры.
    /// </summary>
    public sealed class MainMenuManager : MenuManager
    {          
        /// <summary>
        /// Панель настроек
        /// </summary>
        [SerializeField] private SettingsManager settings;

        //Общие кнопки
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueGameButton;
        [SerializeField] private Button settingsGameButton;
        [SerializeField] private Button exitGameButton;
        //Общие кнопки

        protected override void OnInit()
        {           
            newGameButton.OnClickAsObservable().Subscribe(_ =>
            {
                Missions.PlotState state = new Missions.PlotState();
                string data = JsonConvert.SerializeObject(state);
                File.WriteAllText(Missions.MissionsManager.SavePath, data);

                LoadScreensManager.Instance.LoadScene((int)Scenes.Intro);
            });

            continueGameButton.OnClickAsObservable().Subscribe(_ =>
            {
                Missions.PlotState state = new Missions.PlotState();
                string data = JsonConvert.SerializeObject(state);
                File.WriteAllText(Missions.MissionsManager.SavePath, data);

                LoadScreensManager.Instance.LoadScene((int)Scenes.Intro);
            });

            settingsGameButton.OnClickAsObservable().
                Subscribe(_ =>
                {
                    settings.ShowPanel();
                    genericPanel.SetActive(false);
                });

            exitGameButton.OnClickAsObservable().
                 Subscribe(_ => Application.Quit());
        }        
    }
}