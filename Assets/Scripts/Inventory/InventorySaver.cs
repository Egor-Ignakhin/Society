using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Inventory
{
    /// <summary>
    /// класс отвечающий за сохранение инвентаря
    /// </summary>
    public sealed class InventorySaver
    {
        private readonly string path = Directory.GetCurrentDirectory() + "\\Saves\\Inventory.json";
        public void Load(ref List<InventoryCell> cells) => SaverContainer.Load(path, ref cells);

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
        public List<int> types = new List<int>();// типы предметов в слотах
        public List<int> counts = new List<int>();// кол-во предметов в слотах
        public int cellsCount;// кол-во слотов
        public SaverContainer(List<InventoryCell> cells)
        {
            for (cellsCount = 0; cellsCount < cells.Count; cellsCount++)
            {
                types.Add(cells[cellsCount].Id);// запись ID предмета
                counts.Add(cells[cellsCount].Count);// запись кол-ва предмета                                
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
            catch { Debug.LogError("Load Failed!"); }
        }
    }


}