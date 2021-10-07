using UnityEngine;
using UnityEngine.UI;

namespace PlayerClasses.BasicNeedsEventReceivers
{
    sealed class PlayerThirstDrawer : BasicNeedsEventsReceiver
    {
        #region Fields
        [SerializeField] private Image mImage;
        [SerializeField] private RectTransform mRt;

        #endregion
        #region Подпички-отписки событий
        private void OnEnable() => basicNeeds.ThirstChangeValue += OnChangeThirst;
        private void OnDisable() => basicNeeds.ThirstChangeValue -= OnChangeThirst;
        #endregion

        private void OnChangeThirst(float value) => mImage.fillAmount = value / basicNeeds.MaximumThirst;
    }
}