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
        }        
    }
}