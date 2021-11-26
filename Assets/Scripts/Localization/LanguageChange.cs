using UnityEngine;

namespace Society.Localization
{
    /// <summary>
    /// ������, ������� ���������� TMPro ����� ��� ���������� ��������
    /// </summary>
    internal sealed class LanguageChange : MonoBehaviour
    {
        /// <summary>
        /// ������������ �����
        /// </summary>
        private TMPro.TextMeshProUGUI text;

        /// <summary>
        /// �������� �������������
        /// </summary>
        [SerializeField] private LanguageIdentifiers languageIdentifier;

        private void Awake()
        {
            TryGetComponent(out text);

            //�������� �� ������� ���������� ��������
            Settings.GameSettings.UpdateSettingsEvent += OnSettingsUpdate;
        }

        private void OnSettingsUpdate() => text.SetText(LocalizationManager.Translate(languageIdentifier));

        private void OnDestroy()
        {
            //�������� �� ������� ���������� ��������
            Settings.GameSettings.UpdateSettingsEvent -= OnSettingsUpdate;
        }
    }
}