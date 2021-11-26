using System.Collections;

using Society.Settings;

using UnityEngine;

namespace Society.Menu.Settings
{/// <summary>
/// Уведомление об  успехе сохранения настроек
/// </summary>
    internal sealed class SettingsSaveNotificator : MonoBehaviour
    {
        private TMPro.TextMeshProUGUI mText;
        private Animator mAnimator;
        private void Awake()
        {
            TryGetComponent(out mText);
            TryGetComponent(out mAnimator);

            GameSettings.SaveSettingsEvent += OnSettingsSave;

            gameObject.SetActive(false);
        }

        private IEnumerator WaitForRealSeconds()
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

        private void OnDestroy() => GameSettings.SaveSettingsEvent -= OnSettingsSave;
    }
}