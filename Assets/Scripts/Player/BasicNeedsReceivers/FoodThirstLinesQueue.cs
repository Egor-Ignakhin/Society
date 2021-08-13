using UnityEngine;

namespace PlayerClasses.BasicNeedsReceivers
{
    sealed class FoodThirstLinesQueue : BasicNeedsReceiver
    {
        [SerializeField] Transform baseFood;
        [SerializeField] Transform baseThirst;
        private void OnEnable()
        {
            basicNeeds.FoodChangeValue += OnChangeFoodOrThirst;
            basicNeeds.ThirstChangeValue += OnChangeFoodOrThirst;
        }

        private void OnChangeFoodOrThirst(float _)
        {
            //Предпочтение всегда отдаётся еде
            if (basicNeeds.Food == 0)
            {
                if (baseFood.GetSiblingIndex() != (transform.childCount - 1))
                    baseFood.SetAsLastSibling();
            }
            if (basicNeeds.Thirst == 0)
            {
                if (basicNeeds.Food > 0)
                {
                    if (baseThirst.GetSiblingIndex() != (transform.childCount - 1))
                        baseThirst.SetAsLastSibling();
                }
            }
        }

        private void OnDisable()
        {
            basicNeeds.FoodChangeValue -= OnChangeFoodOrThirst;
            basicNeeds.ThirstChangeValue -= OnChangeFoodOrThirst;
        }
    }
}