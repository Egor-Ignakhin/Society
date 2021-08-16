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
        [SerializeField] private Image add_hp_food;
        [SerializeField] private Image add_hp_thirst;
        #endregion

        #region Подпички-отписки событий
        private void OnEnable() => basicNeeds.HealthChangeValue += OnChangePlayerHealth;

        private void OnDisable() => basicNeeds.HealthChangeValue -= OnChangePlayerHealth;
        #endregion

        private void OnChangePlayerHealth(float health)
        {
            float fillDef = health / (basicNeeds.MaximumHealth * 0.5f);
            float fill_Hp_food = (health > (basicNeeds.MaximumHealth / 2) && basicNeeds.Food > 0) ? (health - (basicNeeds.MaximumHealth * 0.5f)) / (basicNeeds.MaximumHealth / 4) : 0;
            float fill_Hp_thirst = health > (basicNeeds.MaximumHealth / 1.5f) ? (health - (basicNeeds.MaximumHealth * 0.75f)) / (basicNeeds.MaximumHealth / 4) : 0;
            mImage.fillAmount = fillDef;


            add_hp_food.fillAmount = fill_Hp_food;
            add_hp_thirst.fillAmount = fill_Hp_thirst;
        }
    }
}