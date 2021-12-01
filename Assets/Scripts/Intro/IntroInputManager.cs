using System;
using UnityEngine;
using Society.GameScreens;

namespace Society.Intro
{
    public class IntroInputManager : MonoBehaviour
	{
        private const KeyCode skipIntroKeyCode = KeyCode.Space;

        private float skipTimer;
        public event Action<float> SkipTimerEvent;
        public const float SkipTimerNeededSeconds = 3;
        private bool loadingNextSceneIsStarted = false;

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
            if (Input.GetKey(skipIntroKeyCode))
                SetSkipTimer(GetSkipTimer() + Time.deltaTime);

            else if (Input.GetKeyUp(skipIntroKeyCode))
                SetSkipTimer(0);                
        }

        private void OnSkipTimerChange(float seconds)
        {
            if(seconds >= SkipTimerNeededSeconds)
            {
                if (loadingNextSceneIsStarted == false)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene((int)Scenes.Bunker);

                    loadingNextSceneIsStarted = true;
                }
            }
        }

        private void OnDestroy()
        {
            SkipTimerEvent -= OnSkipTimerChange;
        }
    }
}