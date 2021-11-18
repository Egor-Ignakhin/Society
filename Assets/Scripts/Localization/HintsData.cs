using System.Collections.Generic;

namespace Society.Localization
{
    public sealed class HintsData : SerialisableInventoryList
    {
        public List<Hint> Types;
        public Dictionary<string, string> hints;
        public string GetHint(string t) => hints[t];

        [System.Serializable]
        public class Hint : SerialisableInventoryItem
        {
            public string Desc;
        }

    }
}