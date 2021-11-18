using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Society.Localization
{
    public sealed class NutrientItems : SerialisableInventoryList
    {
        public List<Nutritious> MNutritious;
        public Dictionary<int, (int food, int water)> FoodWaterItems;
        [System.Serializable]
        public class Nutritious : SerialisableInventoryItem
        {
            public int Food;
            public int Water;
        }

        internal (int food, int water) GetProperties(int id) => FoodWaterItems[id];
    }
}