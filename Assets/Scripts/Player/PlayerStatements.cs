using UnityEngine;

namespace PlayerClasses
{
    public sealed class PlayerStatements : MonoBehaviour
    {
        private BasicNeeds mBasicNeeds;// класс базовых нужд

        private void Awake()
        {
            mBasicNeeds = GetComponent<BasicNeeds>();
            InputManager.Unlock();
        }
        public void MealPlayer(int food, int water)
        {
            mBasicNeeds.AddMeal(water, food);
        }
    }
}