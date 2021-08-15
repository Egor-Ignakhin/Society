using UnityEngine;

namespace PlayerClasses.BasicNeedsEventReceivers
{
    /// <summary>
    /// Класс меняющий местами линии воды и еды в зависимости от большего параметра.
    /// </summary>
    sealed class FoodThirstLinesQueue : BasicNeedsEventsReceiver
    {
        #region Fields
        [SerializeField] Transform baseFood;// Ссылка на трансформ еды
        [SerializeField] Transform baseThirst;// Ссылка на трансформ воды

        private bool foodIsRight = true;
        #endregion
        #region Подпички-отписки событий
        private void OnEnable()
        {
            basicNeeds.FoodChangeValue += OnChangeFoodOrThirst;
            basicNeeds.ThirstChangeValue += OnChangeFoodOrThirst;
        }

        private void OnDisable()
        {
            basicNeeds.FoodChangeValue -= OnChangeFoodOrThirst;
            basicNeeds.ThirstChangeValue -= OnChangeFoodOrThirst;
        }
        #endregion

        private void OnChangeFoodOrThirst(float _)
        {
            foodIsRight = basicNeeds.Food != 0;

            ChangePositionLines();
        }
        private void ChangePositionLines()
        {
            if (foodIsRight)
                baseFood.SetAsFirstSibling();
            else
                baseThirst.SetAsFirstSibling();
        }
    }
}