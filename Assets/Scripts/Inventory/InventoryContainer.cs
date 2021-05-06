using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public sealed class InventoryContainer : MonoBehaviour
    {
        private readonly Queue<InventoryItem> queueOfItems = new Queue<InventoryItem>();// очередь предметов на отображение
        [SerializeField] private PickUpAndDropDrawer PUDD;// отображетль поднятых п-тов
        [SerializeField] private Transform mainParent;// родитель для отрисовки поверх всего

        public List<InventoryCell> Cells = new List<InventoryCell>();//слоты инвентаря
        private InventoryEventReceiver eventReceiver;
        public readonly List<InventoryCell> HotCells = new List<InventoryCell>();
        private InventoryEffects inventoryEffects;
        private readonly InventorySaver inventorySaver = new InventorySaver();

        [SerializeField] private Transform freeCellsContainer;
        [SerializeField] private Transform busyCellsContainer;
        [SerializeField] private GameObject cellPrefab;

        private void OnEnable()
        {
            eventReceiver = new InventoryEventReceiver(mainParent, FindObjectOfType<FirstPersonController>(), freeCellsContainer, busyCellsContainer, this);
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
                c.Init(this);
            }
            inventorySaver.Load(ref Cells);// загрузка сохранённого инвентаря

            //загрузка слотов для сундуков
            for (int i = 0; i < ItemsContainer.maxCells; i++)
            {
                Instantiate(cellPrefab, freeCellsContainer);
            }

            itemPrefabs = new Dictionary<int, InventoryItem>
            {
                {1, Resources.Load<InventoryItem>("InventoryItems\\Axe_Item_1") },
                {2, Resources.Load<InventoryItem>("InventoryItems\\Makarov_Item_1") },
                {4, Resources.Load<InventoryItem>("InventoryItems\\AK-74u_Item_1") },
                {5, Resources.Load<InventoryItem>("InventoryItems\\CannedFood_Item_1") }

            };
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
            var cell = Cells.Find(c => !c.MItemContainer.IsFilled && c.MItemContainer.Id.Equals(item.Id));

            if (cell == null) cell = Cells.Find(c => c.MItemContainer.IsEmpty);// если слот не нашёлся то запись в пустой слот

            cell.SetItem(item.Id, item.GetCount());

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
            //    PUDD.DrawNewItem(peek.GetId(), peek.GetCount());
        }
        public void ActivateItem()
        {
            eventReceiver.ActivateItem();
        }
        private void OnDisable()
        {
            Save(Cells);
            eventReceiver.OnDisable();
        }
        /// <summary>
        /// сохранение инвентаря
        /// </summary>
        /// <param name="cells"></param>
        private void Save(List<InventoryCell> cells) => inventorySaver.Save(cells);

        public Dictionary<int, InventoryItem> itemPrefabs;
        public InventoryItem GetItemPrefab(int id)
        {
            return itemPrefabs[id];
        }
        public class InventoryEffects
        {
            private readonly AudioClip spendOnCellClip;// звук при наведении на слот
            private readonly AudioSource inventorySpeaker;
            public InventoryEffects(GameObject main)
            {
                spendOnCellClip = Resources.Load<AudioClip>("Inventory\\tic");
                inventorySpeaker = main.AddComponent<AudioSource>();
            }
            public void PlaySpendClip() => inventorySpeaker.PlayOneShot(spendOnCellClip);
        }
    }
}