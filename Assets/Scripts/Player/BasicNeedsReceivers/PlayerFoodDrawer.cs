using UnityEngine;
using UnityEngine.UI;

namespace PlayerClasses.BasicNeedsEventReceivers
{
    sealed class PlayerFoodDrawer : BasicNeedsEventsReceiver
    {
        [SerializeField] private Image mImage;
        [SerializeField] private RectTransform mRt;
        [SerializeField] private RectTransform separator;
        private void OnEnable() => basicNeeds.FoodChangeValue += OnChangeFood;

        private void OnChangeFood(float value)
        {
            var nextPos = separator.anchoredPosition;
            nextPos.x = value * mRt.sizeDelta.x / basicNeeds.MaximumFood;
            separator.anchoredPosition = nextPos;
            mImage.fillAmount = value / basicNeeds.MaximumFood;
        }
        private void OnDisable() => basicNeeds.FoodChangeValue -= OnChangeFood;
    }
}