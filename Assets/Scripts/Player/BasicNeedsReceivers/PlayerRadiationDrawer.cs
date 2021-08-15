using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace PlayerClasses.BasicNeedsEventReceivers
{
    sealed class PlayerRadiationDrawer : BasicNeedsEventsReceiver
    {
        #region Fields
        [SerializeField] private Image mImage;
        [SerializeField] private TextMeshProUGUI text;
        #endregion
        #region Подпички-отписки событий
        private void OnEnable() => basicNeeds.RadiationChangeValue += OnChangeRadiation;
        private void OnDisable() => basicNeeds.RadiationChangeValue -= OnChangeRadiation;
        #endregion

        private void OnChangeRadiation(float value)
        {
            mImage.enabled = value > 1;
            text.enabled = value > 1;
            text.SetText(Mathf.Round(value).ToString());
        }        
    }
}