using UnityEngine;

namespace Society.Player
{
    public sealed class PlayerStatements : MonoBehaviour
    {
        private BasicNeeds mBasicNeeds;// класс базовых нужд

        private void Awake() => mBasicNeeds = GetComponent<BasicNeeds>();

        public void MealPlayer(int food, int water) =>
            mBasicNeeds.AddMeal(water, food);

        internal void HealPlayer(float health, float radiation) =>
            mBasicNeeds.Heal(health, radiation);
    }
}