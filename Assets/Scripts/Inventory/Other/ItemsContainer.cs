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
        [SerializeField] private Types startedType;
        [Range(0, maxCells)] [SerializeField] private int cellsCount;
        private List<(int id, int count, SMGGunAk_74 gun)> container = null;
        public List<(int id, int count, SMGGunAk_74 gun)> GetData() => container;
        private bool isOpened;
        private InventoryEventReceiver inventoryEventReceiver;
        private void Start()
        {
            inventoryEventReceiver = FindObjectOfType<InventoryContainer>().EventReceiver;            
            SetType(startedType.ToString());
        }

        public override void Interact(PlayerStatements pl)
        {
            if (isOpened)
                return;
            inventoryEventReceiver.OpenContainer(container, cellsCount, this);
            isOpened = true;
        }
        /// <summary>
        /// метод закрывает сохраняет ячейки в памяти
        /// </summary>
        public void Close(List<(int id, int count, SMGGunAk_74 gun)> lst)
        {
            container = lst;
            isOpened = false;
        }
    }

}