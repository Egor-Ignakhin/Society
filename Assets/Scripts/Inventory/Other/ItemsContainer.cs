using PlayerClasses;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// класс-контейнер (сундук, ящик и тп)
    /// </summary>
    public class ItemsContainer : InteractiveObject
    {
        public const int maxCells = 40;
        [Range(0, maxCells)] [SerializeField] private int cellsCount;
        private List<(int id, int count)> container = null;
        private bool isOpened;
        public override void Interact(PlayerStatements pl)
        {
            if (isOpened)
                return;
            InventoryEventReceiver.Instance.OpenContainer(container, cellsCount, this);
            isOpened = true;
        }
        /// <summary>
        /// метод закрывает сохраняет ячейки в памяти
        /// </summary>
        public void Close(List<(int id, int count)> lst)
        {
            container = lst;
            isOpened = false;
        }
    }

}