using System.Collections.Generic;
using UnityEngine;
using static SMG.ModifierCharacteristics;

namespace SMG
{
    public class SMGModifiersData : MonoBehaviour
    {
        private List<SMGTitleTypeIndex> data = new List<SMGTitleTypeIndex>();

        private SMGSaver saver;
        private readonly string savingPath = System.IO.Directory.GetCurrentDirectory() + "\\Saves\\SMGSave.json";
        private void OnEnable()
        {
            saver = new SMGSaver();
            saver.Load(ref data, savingPath);
        }

        public List<SMGTitleTypeIndex> GetModifiersData() => data;
        internal void AddModifier(SMGTitleTypeIndex tti) => data.Add(tti);

        private void OnDisable() => saver.Save(data, savingPath);
    }
}