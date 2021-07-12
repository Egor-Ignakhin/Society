using PlayerClasses;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// класс-контейнер (сундук, ящик и тп)
    /// </summary>
    public sealed class ItemsContainer : InteractiveObject
    {
        public const int maxCells = 40;
        [SerializeField] private Types startedType;
        [Range(0, maxCells)] [SerializeField] private int cellsCount;
        private List<(int id, int count, SMGInventoryCellGun gun)> container = new List<(int id, int count, SMGInventoryCellGun gun)>();
        public List<(int id, int count, SMGInventoryCellGun gun)> GetData() => container;
        private bool isOpened;
        private InventoryEventReceiver inventoryEventReceiver;
        [SerializeField] private List<int> startedItems = new List<int>();
        [SerializeField] private List<int> startedCount = new List<int>();
        public (List<int> items, List<int> count) GetStartedData() => (startedItems, startedCount);
        private void Start()
        {
            inventoryEventReceiver = FindObjectOfType<InventoryContainer>().EventReceiver;
            for (int i = 0; i < startedItems.Count; i++)
            {
                var possibleGun = new SMGInventoryCellGun();
                possibleGun.Reload(null);
                container.Add((startedItems[i], startedCount[i], possibleGun));
            }
            SetType(startedType.ToString());
        }

        public override void Interact()
        {
            if (isOpened)
                return;
            inventoryEventReceiver.OpenContainer(container, cellsCount, this);
            isOpened = true;
        }
        /// <summary>
        /// метод закрывает сохраняет ячейки в памяти
        /// </summary>
        public void Close(List<(int id, int count, SMGInventoryCellGun gun)> lst)
        {
            container = lst;
            isOpened = false;
        }
        private void OnValidate()
        {
            startedType = Types.Container_1;

            if (startedItems.Count < cellsCount)
            {
                for (int i = startedItems.Count; i < cellsCount; i++)
                {
                    startedItems.Add(0);
                    startedCount.Add(1);
                }
            }
            else
            {
                while (startedItems.Count > cellsCount)
                {
                    startedItems.RemoveAt(startedItems.Count - 1);
                    startedCount.RemoveAt(startedCount.Count - 1);
                }
            }
            //фикс возможных проблем связанных с "количеством пустого слота"
            for (int i = 0; i < startedItems.Count; i++)
            {
                if ((startedCount[i] > 0) &&
                    (startedItems[i] == 0))
                    startedCount[i] = 0;
            }
        }
        private void Reset()
        {
            startedType = Types.Container_1;
        }
    }
}