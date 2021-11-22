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
    public sealed class MainMenuManager : MenuManager
    {
        private AudioClip OnButtonEnterSound;
        private AudioSource aud;
        [SerializeField] private SettingsManager settings;


        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueGameButton;
        [SerializeField] private Button settingsGameButton;
        [SerializeField] private Button exitGameButton;

        private void Awake()
        {
            OnButtonEnterSound = Resources.Load<AudioClip>("Inventory\\tic_2");
            aud = gameObject.AddComponent<AudioSource>();
            aud.volume = 0.5f;            

            newGameButton.OnClickAsObservable().Subscribe(_ =>
            {
                Missions.PlotState state = new Missions.PlotState();
                string data = JsonConvert.SerializeObject(state);
                File.WriteAllText(Missions.MissionsManager.SavePath, data);

                SceneManager.LoadScene((int)Scenes.Intro);
            });

            continueGameButton.OnClickAsObservable().Subscribe(_ =>
            {
                Missions.PlotState state = new Missions.PlotState();
                string data = JsonConvert.SerializeObject(state);
                File.WriteAllText(Missions.MissionsManager.SavePath, data);

                SceneManager.LoadScene((int)Scenes.Intro);
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

        internal void OnButtonEnter() => aud.PlayOneShot(OnButtonEnterSound);
    }
}