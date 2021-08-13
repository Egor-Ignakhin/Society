using UnityEngine;
using UnityEngine.UI;

namespace PlayerClasses.BasicNeedsReceivers
{
    public class PlayerThirstDrawer : BasicNeedsReceiver
    {
        [SerializeField] private Image mImage;
        [SerializeField] private RectTransform mRt;
        [SerializeField] private RectTransform separator;
        private void OnEnable() => basicNeeds.ThirstChangeValue += OnChangeThirst;

        private void OnChangeThirst(float value)
        {
            var nextPos = separator.anchoredPosition;
            nextPos.x = value * mRt.sizeDelta.x / basicNeeds.MaximumThirst;
            separator.anchoredPosition = nextPos;
            mImage.fillAmount = value / basicNeeds.MaximumThirst;
        }
        private void OnDisable() => basicNeeds.ThirstChangeValue -= OnChangeThirst;
    }
}