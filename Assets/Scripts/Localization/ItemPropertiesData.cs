using System.Collections.Generic;

namespace Society.Localization
{
    public sealed class ItemPropertiesData : SerialisableInventoryList
    {
        public List<ItemProperties> Properties;
        public Dictionary<int, (int maxCount, decimal weight)> WeightAndMaxCountItems;
        public int GetMaxCount(int id) => WeightAndMaxCountItems[id].maxCount;
        public decimal GetWight(int id) => WeightAndMaxCountItems[id].weight;

        [System.Serializable]
        public class ItemProperties : SerialisableInventoryItem
        {
            public int MaxCount;
            public float Weight;
        }
    }
}