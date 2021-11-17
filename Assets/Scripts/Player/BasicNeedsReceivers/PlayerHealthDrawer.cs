using Society.Player;

using UnityEngine;
using UnityEngine.UI;

namespace PlayerClasses.BasicNeedsEventReceivers
{
    /// <summary>
    /// Класс отображает текущее здоровье в виде: полоса(0.5)-полоса(0.25)-полоса(0.25) + разделитель
    /// </summary>
    internal sealed class PlayerHealthDrawer : BasicNeedsEventsReceiver
    {
        #region Fields       
        [SerializeField] private Image mImage;// большая левая строка здоровья                
        [SerializeField] private Image add_hp_food;
        [SerializeField] private Image add_hp_thirst;


        #endregion

        #region Подпички-отписки событий
        private void OnEnable()
        {
            basicNeeds.HealthChangeValue += OnChangePlayerHealth;
            basicNeeds.FoodChangeValue += OnChangeFoodOrThirst;
            basicNeeds.ThirstChangeValue += OnChangeFoodOrThirst;
        }

        private void OnDisable()
        {
            basicNeeds.HealthChangeValue -= OnChangePlayerHealth;
            basicNeeds.FoodChangeValue -= OnChangeFoodOrThirst;
            basicNeeds.ThirstChangeValue -= OnChangeFoodOrThirst;
        }
        #endregion

        private void OnChangePlayerHealth(float health)
        {
            float fillDef = health / (basicNeeds.MaxHealth * 0.5f);
            float fill_Hp_food = (basicNeeds.Food > 0 || (basicNeeds.Thirst > 0)) && (health > (basicNeeds.MaxHealth / 2)) ? ((health - (basicNeeds.MaxHealth * 0.5f)) / (basicNeeds.MaxHealth / 4)) : 0;
            float fill_Hp_thirst = (basicNeeds.Food > 0) && (health > (basicNeeds.MaxHealth / 1.5f)) ? ((health - (basicNeeds.MaxHealth * 0.75f)) / (basicNeeds.MaxHealth / 4)) : 0;
            mImage.fillAmount = fillDef;


            add_hp_food.fillAmount = fill_Hp_food;
            add_hp_thirst.fillAmount = fill_Hp_thirst;
        }

        private void OnChangeFoodOrThirst(float _)
        {
            if (basicNeeds.Food == 0)
            {
                if (basicNeeds.Health > basicNeeds.MaxHealth * 0.75f)// Если хп больше 75, вода есть, а еды нет
                {
                    BasicNeeds.ForceSetHealth((int)(basicNeeds.MaxHealth * 0.75f));
                }
                if (basicNeeds.Thirst == 0)
                {
                    if (basicNeeds.Health > basicNeeds.MaxHealth * 0.5f)
                    {
                        BasicNeeds.ForceSetHealth((int)(basicNeeds.MaxHealth * 0.5f));
                    }
                }
            }
            if (basicNeeds.Thirst == 0)
            {
                if (basicNeeds.Health > basicNeeds.MaxHealth * 0.75f)// Если хп больше 75, вода есть, а еды нет
                {
                    BasicNeeds.ForceSetHealth((int)(basicNeeds.MaxHealth * 0.75f));
                }
                if (basicNeeds.Food == 0)
                {
                    if (basicNeeds.Health > basicNeeds.MaxHealth * 0.5f)
                    {
                        BasicNeeds.ForceSetHealth((int)(basicNeeds.MaxHealth * 0.5f));
                    }
                }
            }
            OnChangePlayerHealth(basicNeeds.Health);
        }
    }
}