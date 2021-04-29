using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Inventory
{
    public sealed class InventoryContainer : Singleton<InventoryContainer>
    {
        private readonly Queue<InventoryItem> queueOfItems = new Queue<InventoryItem>();// очередь предметов на отображение
        [SerializeField] private PickUpAndDropDrawer PUDD;// отображетль поднятых п-тов
        [SerializeField] private Transform mainParent;// родитель для отрисовки поверх всего

        public List<InventoryCell> Cells = new List<InventoryCell>();//слоты инвентаря
        private InventoryEventReceiver eventReceiver;
        public readonly List<InventoryCell> HotCells = new List<InventoryCell>();
        private InventoryEffects inventoryEffects;
        private InventorySaver inventorySaver = new InventorySaver();
        private void OnEnable()
        {
            eventReceiver = new InventoryEventReceiver(mainParent, FindObjectOfType<FirstPersonController>());
            eventReceiver.OnEnable();
            inventoryEffects = new InventoryEffects(gameObject);
        }
        private void Start()
        {
            // добавление всех ячеек в список
            var mc = InventoryDrawer.Instance.GetMainContainer();
            foreach (var cell in mc.GetComponentsInChildren<InventoryCell>())
            {
                Cells.Add(cell);
            }
            var sc = InventoryDrawer.Instance.GetSupportContainer();
            foreach (var cell in sc.GetComponentsInChildren<InventoryCell>())
            {
                Cells.Add(cell);
                HotCells.Add(cell);
            }
            foreach (var c in Cells)
            {
                c.Init();
            }
            inventorySaver.Load(ref Cells);
        }
        /// <summary>
        /// добавление поднятого предмета в очередь
        /// </summary>
        /// <param name="item"></param>    
        public void AddItem(InventoryItem item)
        {
            if (Cells.FindAll(c => c.MItemContainer.IsEmpty).Count == 0)// если не нашлись свободные слоты
                return;

            // поиск слота, с предметом того же типа, и не заполненным   
            var cell = Cells.Find(c => !c.MItemContainer.IsFilled && c.MItemContainer.Type.Equals(item.GetObjectType()));

            if (cell == null) cell = Cells.Find(c => c.MItemContainer.IsEmpty);// если слот не нашёлся то запись в пустой слот

            cell.SetItem(item);

            queueOfItems.Enqueue(item);// добавить предмет в очередь
            MessageToPUDD();
        }
        public void SpendOnCell()
        {
            inventoryEffects.PlaySpendClip();
        }
        private void MessageToPUDD()
        {
            var peek = queueOfItems.Dequeue();
            PUDD.DrawNewItem(peek.GetObjectType(), peek.GetCount());
        }

        private void OnDisable()
        {
            Save(Cells);
            eventReceiver.OnDisable();
        }
        private void Save(List<InventoryCell> cells)
        {
            inventorySaver.Save(cells);
        }
        public class InventoryEffects
        {
            private AudioClip spendOnCellClip;
            private AudioSource inventorySpeaker;
            public InventoryEffects(GameObject main)
            {
                spendOnCellClip = Resources.Load<AudioClip>("Inventory\\tic");
                inventorySpeaker = main.AddComponent<AudioSource>();
            }
            public void PlaySpendClip()
            {
                inventorySpeaker.PlayOneShot(spendOnCellClip);
            }
        }
    }
}