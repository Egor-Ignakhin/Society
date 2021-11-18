using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Society.Localization
{
    public sealed class MedicalItems : SerialisableInventoryList
    {
        public List<Medicals> Items;
        public Dictionary<int, (int health, int radiation)> medItems;
        [System.Serializable]
        public class Medicals : SerialisableInventoryItem
        {
            public int Health;
            public int Radiation;
        }

        internal (int health, int radiation) GetSalubrity(int id) => medItems[id];
    }
}