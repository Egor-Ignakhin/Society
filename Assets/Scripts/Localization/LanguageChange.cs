using UnityEngine;

namespace Society.Localization
{
    internal sealed class LanguageChange : MonoBehaviour
    {
        private TMPro.TextMeshProUGUI text;

        [SerializeField] private LanguageIdentifiers languageIdentifier;

        private void Awake()
        {
            TryGetComponent(out text);

            Menu.Settings.SettingsManager.SettingsUpdateEvent += OnSettingsUpdate;
        }

        private void OnSettingsUpdate()
        {
            text.SetText(LocalizationManager.Translate(languageIdentifier));
        }

        private void OnDestroy()
        {
            Menu.Settings.SettingsManager.SettingsUpdateEvent -= OnSettingsUpdate;
        }
    }
}