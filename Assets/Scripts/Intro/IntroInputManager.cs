using System;

using Society.GameScreens;

using UnityEngine;
using UnityEngine.Video;

namespace Society.Intro
{
    public class IntroInputManager : MonoBehaviour
    {
        private const KeyCode skipIntroKeyCode = KeyCode.Space;

        private float skipTimer;
        public event Action<float> SkipTimerEvent;
        public const float SkipTimerNeededSeconds = 3;
        private bool loadingNextSceneIsStarted = false;
        [SerializeField] private VideoPlayer videoPlayer;

        private float timeFromStart;

        public float GetSkipTimer() => skipTimer;

        public void SetSkipTimer(float value)
        {
            skipTimer = value;

            SkipTimerEvent?.Invoke(skipTimer);
        }

        private void Awake()
        {
            SkipTimerEvent += OnSkipTimerChange;
        }

        private void Start()
        {
            SetSkipTimer(0);
        }
        private void Update()
        {
            timeFromStart += Time.deltaTime;

            if (Input.GetKey(skipIntroKeyCode))
                SetSkipTimer(GetSkipTimer() + Time.deltaTime);

            else if (Input.GetKeyUp(skipIntroKeyCode))
                SetSkipTimer(0);

            if (timeFromStart >= videoPlayer.length)
            {
                //Video has finshed playing!

                LoadNextScene();
            }
        }

        private void OnSkipTimerChange(float seconds)
        {
            if (seconds >= SkipTimerNeededSeconds)
            {
                LoadNextScene();
            }
        }

        private void LoadNextScene()
        {
            if (loadingNextSceneIsStarted == false)
            {
                LoadScreensManager.Instance.LoadScene((int)Scenes.Bunker);

                loadingNextSceneIsStarted = true;
            }
        }

        private void OnDestroy()
        {
            SkipTimerEvent -= OnSkipTimerChange;
        }
    }
}