using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace PlayerClasses.BasicNeedsReceivers
{
    sealed class PlayerRadiationDrawer : BasicNeedsReceiver
    {
        [SerializeField] private Image mImage;
        [SerializeField] private TextMeshProUGUI text;
        private void OnEnable() => basicNeeds.RadiationChangeValue += OnChangeRadiation;

        private void OnChangeRadiation(float value)
        {
            mImage.enabled = value > 1;
            text.enabled = value > 1;
            text.SetText(Mathf.Round(value).ToString());
        }
        private void OnDisable() => basicNeeds.RadiationChangeValue -= OnChangeRadiation;
    }
}