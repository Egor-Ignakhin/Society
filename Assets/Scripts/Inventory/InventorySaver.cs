using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Inventory
{
    public class InventorySaver
    {
        private string path = Directory.GetCurrentDirectory() + "\\Saves\\Inventory.json";
        public void Load(ref List<InventoryCell> cells)
        {
            SaverContainer.Load(path, ref cells);
        }
        public void Save(List<InventoryCell> cells)
        {
            // Start save date
            SaverContainer container = new SaverContainer(cells);
            string savingDate = JsonUtility.ToJson(container, true);
            File.WriteAllText(path, savingDate);
            // End save date
        }
    }
    [System.Serializable]
    public class SaverContainer
    {
        public List<int> types = new List<int>();
        public List<int> counts = new List<int>();
        public int cellsCount;
        public SaverContainer(List<InventoryCell> cells)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                types.Add(cells[i].MItemContainer.Id);// запись ID предмета
                counts.Add(cells[i].MItemContainer.Count);// запись кол-ва предмета                
                cellsCount++;
            }
        }
        /// <summary>
        /// при запуске этого метода происходит загрузка данных о слотах
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cells"></param>
        public static void Load(string path, ref List<InventoryCell> cells)
        {
            try// ошибка возникнет при отсутствующем файле сохранения
            {
                string data = File.ReadAllText(path);
                SaverContainer c = JsonUtility.FromJson<SaverContainer>(data);
                for (int i = 0; i < c.cellsCount; i++)
                {
                    cells[i].SetItem(c.types[i], c.counts[i]);
                }
            }
            catch { }
        }
    }


}