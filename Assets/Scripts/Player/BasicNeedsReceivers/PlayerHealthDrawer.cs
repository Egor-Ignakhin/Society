using UnityEngine;
using UnityEngine.UI;

namespace PlayerClasses.BasicNeedsEventReceivers
{
    /// <summary>
    /// Класс отображает текущее здоровье в виде: полоса(0.5)-полоса(0.25)-полоса(0.25) + разделитель
    /// </summary>
    sealed class PlayerHealthDrawer : BasicNeedsEventsReceiver
    {
        #region Fields
        [SerializeField] private RectTransform separator;// Разделящая строки здоровья линия

        [SerializeField] private Image mImage;// большая левая строка здоровья        
        [SerializeField] private Image additionalHp_1;
        [SerializeField] private Image additionalHp_2;
        #endregion

        #region Подпички-отписки событий
        private void OnEnable() => basicNeeds.HealthChangeValue += OnChangePlayerHealth;

        private void OnDisable() => basicNeeds.HealthChangeValue -= OnChangePlayerHealth;
        #endregion

        private void OnChangePlayerHealth(float health)
        {
            float fillDef = health / (basicNeeds.MaximumHealth * 0.5f);
            float fill_Hp_1 = health > (basicNeeds.MaximumHealth / 2) ? (health - (basicNeeds.MaximumHealth * 0.5f)) / (basicNeeds.MaximumHealth / 4) : 0;
            float fill_Hp_2 = health > (basicNeeds.MaximumHealth / 1.5f) ? (health - (basicNeeds.MaximumHealth * 0.75f)) / (basicNeeds.MaximumHealth / 4) : 0;
            mImage.fillAmount = fillDef;

            additionalHp_1.fillAmount = fill_Hp_1;
            additionalHp_2.fillAmount = fill_Hp_2;


            Transform parentSeparator = mImage.fillAmount < 1 ? mImage.transform : (additionalHp_1.fillAmount < 1 ? additionalHp_1.transform : additionalHp_2.transform);
            float fillAmountSeparator = mImage.fillAmount < 1 ? fillDef : (additionalHp_1.fillAmount < 1 ? fill_Hp_1 : fill_Hp_2);
            SetSeparatorState(parentSeparator, fillAmountSeparator);
        }

        /// <summary>
        /// Установка разделителя в место обрезания линий здоровья
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="fillAmount"></param>
        private void SetSeparatorState(Transform parent, float fillAmount)
        {
            if (separator.parent != parent)
                separator.SetParent(parent);
            separator.anchorMin = new Vector2(fillAmount, separator.anchorMin.y);
            separator.anchorMax = new Vector2(fillAmount, separator.anchorMax.y);
            separator.anchoredPosition = Vector2.zero;
        }
    }
}