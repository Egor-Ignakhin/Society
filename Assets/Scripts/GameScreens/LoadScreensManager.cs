using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Society.GameScreens
{
    internal sealed class LoadScreensManager : MonoBehaviour, IGameScreen
    {
        [SerializeField] private GameObject canvas;

        public static LoadScreensManager Instance;

        private static AsyncOperation currentAsyncLoad;

        [SerializeField] private Animator mAnimator;

        private int nextScene;

        [SerializeField] private Transform prefabs;

        private GameObject activePrefab;

        [SerializeField] private Image progressImage;
        [SerializeField] private GameObject operationProgress;

        private void Awake()
        {
            Instance = this;



            DontDestroyOnLoad(gameObject);

            canvas.SetActive(false);

            DisableAllUIElements();
        }

        public void LoadScene(int buildIndex)
        {
            ScreensManager.SetScreen(this);

            canvas.SetActive(true);

            transform.SetAsLastSibling();

            mAnimator.enabled = true;
            mAnimator.SetTrigger("OnStartLoadScene");

            nextScene = buildIndex;
        }
        public void ShowPictures()
        {
            StartCoroutine(nameof(ImagesChanger));
        }

        public void StartAsyncLoading()
        {           
            Resources.UnloadUnusedAssets();
            System.GC.Collect(2);

            currentAsyncLoad = SceneManager.LoadSceneAsync(nextScene);

            currentAsyncLoad.completed += OnLoadCompleted;

            operationProgress.SetActive(true);
            StartCoroutine(nameof(ProgressLineUpdater));
        }

        public bool Hide() => false;

        public KeyCode HideKey() => KeyCode.Escape;

        private IEnumerator ImagesChanger()
        {
            while (true)
            {
                int randomIndex = Random.Range(0, prefabs.childCount);

                if (activePrefab)
                {
                    while (randomIndex == activePrefab.transform.GetSiblingIndex())
                        randomIndex = Random.Range(0, prefabs.childCount);
                }


                for (int i = 0; i < prefabs.childCount; i++)
                {
                    prefabs.GetChild(i).gameObject.SetActive(false);
                }
                activePrefab = prefabs.GetChild(randomIndex).gameObject;


                activePrefab.SetActive(true);

                yield return new WaitForSeconds(15);
            }
        }
        private IEnumerator ProgressLineUpdater()
        {
            while (true)
            {
                progressImage.fillAmount = currentAsyncLoad.progress;

                yield return new WaitForEndOfFrame();
            }
        }
        private void OnLoadCompleted(AsyncOperation _)
        {
            mAnimator.SetTrigger("OnFinishLoadScene");                    
        }

        public void DisableAllUIElements()
        {
            activePrefab = null;
            operationProgress.SetActive(false);

            StopAllCoroutines();

            for (int i = 0; i < prefabs.childCount; i++)
            {
                prefabs.GetChild(i).gameObject.SetActive(false);
            }

        }
    }
}