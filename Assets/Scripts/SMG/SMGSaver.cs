using System.Collections.Generic;

using UnityEngine;

using static Society.SMG.ModifierCharacteristics;

namespace Society.SMG
{
    public class SMGSaver
    {
        [System.Serializable]
        private class SMGModifiersSaveSupporter
        {
            public List<int> titles = new List<int>();
            public List<int> types = new List<int>();
            public List<int> indexes = new List<int>();

            public int modifiersCount;
            public SMGModifiersSaveSupporter(List<SMGTitleTypeIndex> data)
            {
                for (modifiersCount = 0; modifiersCount < data.Count; modifiersCount++)
                {
                    titles.Add((int)data[modifiersCount].Title);
                    types.Add((int)data[modifiersCount].Type);
                    indexes.Add((int)data[modifiersCount].Index);
                }
            }
            public static void GetData(ref List<SMGTitleTypeIndex> data, string path)
            {
                try// ошибка возникнет при отсутствующем файле сохранения
                {
                    string dataJson = System.IO.File.ReadAllText(path);
                    SMGModifiersSaveSupporter c = JsonUtility.FromJson<SMGModifiersSaveSupporter>(dataJson);
                    for (int i = 0; i < c.modifiersCount; i++)
                    {
                        data.Add(new SMGTitleTypeIndex((GunTitles)c.titles[i], (ModifierTypes)c.types[i], (ModifierIndex)c.indexes[i]));
                    }
                }
                catch { Debug.LogError("Load Failed (load modifiers)!"); }
            }
        }
        public void Load(ref List<SMGTitleTypeIndex> data, string path) => SMGModifiersSaveSupporter.GetData(ref data, path);

        internal void Save(List<SMGTitleTypeIndex> data, string path)
        {
            SMGModifiersSaveSupporter support = new SMGModifiersSaveSupporter(data);
            string savedData = JsonUtility.ToJson(support, true);
            System.IO.File.WriteAllText(path, savedData);
        }
    }
}