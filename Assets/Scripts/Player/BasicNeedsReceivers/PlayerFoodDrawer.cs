using UnityEngine;
using UnityEngine.UI;

namespace PlayerClasses.BasicNeedsEventReceivers
{
    internal sealed class PlayerFoodDrawer : BasicNeedsEventsReceiver
    {
        [SerializeField] private Image mImage;
        [SerializeField] private RectTransform mRt;
        private void OnEnable() => basicNeeds.FoodChangeValue += OnChangeFood;

        private void OnChangeFood(float value)
        {

            mImage.fillAmount = value / basicNeeds.MaxFood;
        }

        private void OnDisable() => basicNeeds.FoodChangeValue -= OnChangeFood;
    }
}