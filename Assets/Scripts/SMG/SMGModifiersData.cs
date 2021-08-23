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
        private const int maxDataCount = 20;
        private Inventory.InventoryInput inventoryInput;
        private void OnEnable()
        {
            inventoryInput = FindObjectOfType<Inventory.InventoryContainer>().MInventoryInput;
            saver = new SMGSaver();
            saver.Load(ref data, savingPath);
        }

        public List<SMGTitleTypeIndex> GetModifiersData() => data;
        internal void AddModifier(SMGTitleTypeIndex tti)
        {
            if (tti.Index != ModifierIndex.None)
            {                
                if (data.Count < maxDataCount)//если в контейнере есть место
                    data.Add(tti);
                else
                    inventoryInput.DropModifier(tti);
            }
        }

        private void OnDisable() => saver.Save(data, savingPath);

        internal void RemoveModifier(SMGTitleTypeIndex tti)
        {
            data.Remove(tti);
        }
    }
}