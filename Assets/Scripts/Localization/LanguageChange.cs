using UnityEngine;

namespace Society.Localization
{
    /// <summary>
    /// Скрипт, который локализует TMPro текст при обновлении настроек
    /// </summary>
    internal sealed class LanguageChange : MonoBehaviour
    {
        /// <summary>
        /// Локализуемый текст
        /// </summary>
        private TMPro.TextMeshProUGUI text;

        /// <summary>
        /// Языковой идентификатор
        /// </summary>
        [SerializeField] private LanguageIdentifiers languageIdentifier;

        private void Awake()
        {
            TryGetComponent(out text);

            //Подписка на событие обновления настроек
            Settings.GameSettings.UpdateSettingsEvent += OnSettingsUpdate;
        }

        private void OnSettingsUpdate() => text.SetText(LocalizationManager.Translate(languageIdentifier));

        private void OnDestroy()
        {
            //Подписка от события обновления настроек
            Settings.GameSettings.UpdateSettingsEvent -= OnSettingsUpdate;
        }
    }
}