using UnityEngine;
using UnityEngine.UI;

namespace PlayerClasses.BasicNeedsReceivers
{
    sealed class PlayerValueDrawer : BasicNeedsReceiver
    {
        [SerializeField] private RectTransform separator;

        [SerializeField] private Image mImage;
        [SerializeField] private RectTransform mrt;
        [SerializeField] private Image additionalHp_1;
        [SerializeField] private Image additionalHp_2;


        private void OnEnable() => basicNeeds.HealthChangeValue += OnChangePlayerHealth;

        private void OnDisable() => basicNeeds.HealthChangeValue -= OnChangePlayerHealth;

        private void OnChangePlayerHealth(float _)
        {
            float fillDef = basicNeeds.Health / (basicNeeds.MaximumHealth * 0.5f);
            float fill_Hp_1 = basicNeeds.Health > (basicNeeds.MaximumHealth / 2) ? (basicNeeds.Health - (basicNeeds.MaximumHealth * 0.5f)) / (basicNeeds.MaximumHealth / 4) : 0;
            float fill_Hp_2 = basicNeeds.Health > (basicNeeds.MaximumHealth / 1.5f) ? (basicNeeds.Health - (basicNeeds.MaximumHealth * 0.75f)) / (basicNeeds.MaximumHealth / 4) : 0;
            mImage.fillAmount = fillDef;

            additionalHp_1.fillAmount = fill_Hp_1;
            additionalHp_2.fillAmount = fill_Hp_2;


            Transform parentSeparator = mImage.fillAmount < 1 ? mrt : (additionalHp_1.fillAmount < 1 ? additionalHp_1.transform : additionalHp_2.transform);
            float fillAmountSeparator = mImage.fillAmount < 1 ? fillDef : (additionalHp_1.fillAmount < 1 ? fill_Hp_1 : fill_Hp_2);
            SetSeparatorState(parentSeparator, fillAmountSeparator);
        }
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