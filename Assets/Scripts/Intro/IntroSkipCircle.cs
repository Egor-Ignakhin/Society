using UnityEngine;

namespace Society.Intro
{
    internal sealed class IntroSkipCircle : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Image mCircleImage;
        [SerializeField] private IntroInputManager IntroInputManager;


        private void Awake()
        {
            IntroInputManager.SkipTimerEvent += OnIntroManagerSkipTimer;
        }

        private void OnIntroManagerSkipTimer(float seconds)
        {
            mCircleImage.fillAmount = seconds / IntroInputManager.SkipTimerNeededSeconds;
        }
        private void OnDestroy()
        {
            IntroInputManager.SkipTimerEvent -= OnIntroManagerSkipTimer;
        }
    }
}