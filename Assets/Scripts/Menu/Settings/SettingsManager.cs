
using System;

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
    }
}