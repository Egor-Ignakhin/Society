using PlayerClasses;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class ItemsContainer : InteractiveObject
    {
        public const int maxCells = 10;
        [Range(0, maxCells)] [SerializeField] private int cellsCount;
        private List<(int id, int count)> container;
        private bool isOpened;
        protected override void Awake()
        {
            container = null;
        }
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