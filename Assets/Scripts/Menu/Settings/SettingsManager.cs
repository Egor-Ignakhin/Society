
using UniRx;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Menu.Settings
{
    public class SettingsManager : MonoBehaviour
    {
        #region SubPanels

        [SerializeField] private Button subpanelGame;
        [SerializeField] private Button subpanelInput;
        [SerializeField] private Button subpanelVideo;

        #endregion

        [Space(5)]

        #region GenericButtons

        [SerializeField] private Button backButton;
        [SerializeField] private Button applyButton;

        #endregion

        #region Dynamic

        private GameObject activeSubpanel;

        #endregion

        [SerializeField] private MenuManager menuManager;

        private void Awake()
        {
            subpanelGame.OnClickAsObservable().Subscribe(_ => Debug.Log("Test UniRx"));
            backButton.OnClickAsObservable().Subscribe(_ =>
            {
                gameObject.SetActive(false);
                menuManager.ShowGenericPanel();
            });
        }

        private void ShowSubpanel(GameObject subpanel)
        {
            if (activeSubpanel)
                activeSubpanel.SetActive(false);

            activeSubpanel = subpanel;
            activeSubpanel.SetActive(true);
        }
    }
}