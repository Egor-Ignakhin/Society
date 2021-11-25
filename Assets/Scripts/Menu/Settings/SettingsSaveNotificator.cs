using System;
using System.Collections;
using System.Threading;

using UniRx;
using UnityEngine;

namespace Society.Menu.Settings
{
    internal sealed class SettingsSaveNotificator : MonoBehaviour
    {
        private TMPro.TextMeshProUGUI mText;
        private Animator mAnimator;
        private void Awake()
        {
            TryGetComponent(out mText);
            TryGetComponent(out mAnimator);

            SettingsManager.SaveSettingsEvent += OnSettingsSave;

            gameObject.SetActive(false);
        }
        private void DisableObject()
        {
            gameObject.SetActive(false);
        }

        IEnumerator WaitForRealSeconds()
        {
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < 3)
            {
                yield return null;
            }
            gameObject.SetActive(false);
        }

        private void OnSettingsSave()
        {
            StopAllCoroutines();
            mText.SetText("OPTIONS APPLIED!");
            gameObject.SetActive(true);

            StartCoroutine(nameof(WaitForRealSeconds));           
        }
        private void OnDestroy()
        {
            SettingsManager.SaveSettingsEvent -= OnSettingsSave;
        }
    }
}