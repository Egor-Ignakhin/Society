using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

namespace Inventory
{
    /// <summary>
    /// главный контейнер инвентаря
    /// </summary>
    public sealed class InventoryContainer : MonoBehaviour
    {
        [SerializeField] private Transform mainParent;// родитель для отрисовки поверх всего

        private List<InventoryCell> Cells = new List<InventoryCell>();//слоты инвентаря
        public List<InventoryCell> GetCells() => Cells;
        public readonly List<RectTransform> CellsRect = new List<RectTransform>();
        private InventoryEventReceiver eventReceiver;
        public InventoryEventReceiver EventReceiver
        {
            get
            {
                if (eventReceiver == null)
                {
                    eventReceiver = new InventoryEventReceiver(mainParent, FindObjectOfType<FirstPersonController>(), freeCellsContainer,
                busyCellsContainer, this, ItemsLabelDescription, MInventoryInput, inventoryDrawer, weightText, takeAllButton,
                ModifiersActivator, modifiersPage, FindObjectOfType<SMG.SMGInventoryCellsEventReceiver>());
                }
                return eventReceiver;
            }
        }

        private readonly List<InventoryCell> HotCells = new List<InventoryCell>();
        public List<InventoryCell> GetHotCells() => HotCells;
        private InventorySoundEffects inventorySoundEffects;

        private readonly InventorySaver inventorySaver = new InventorySaver();
        [SerializeField] private Transform freeCellsContainer;
        [SerializeField] private Transform busyCellsContainer;
        [SerializeField] private GameObject cellPrefab;
        private PlayerClasses.PlayerStatements playerStatements;
        [SerializeField] private GameObject ItemsLabelDescription;
        [SerializeField] private TextMeshProUGUI weightText;
        [SerializeField] private Button ModifiersActivator;
        [SerializeField] private GameObject modifiersPage;
        [SerializeField] private Button takeAllButton;// кнопка у слотов контейнеров, забирает всё, что можно        

        private InventoryInput inventoryInput;
        public InventoryInput MInventoryInput
        {
            get
            {
                if (inventoryInput == null)
                    inventoryInput = gameObject.AddComponent<InventoryInput>();
                return inventoryInput;
            }
            private set => inventoryInput = value;
        }

        private InventoryDrawer inventoryDrawer;

        public delegate void InteractiveHandler(int id, int count);
        public event InteractiveHandler TakeItemEvent;
        public event InteractiveHandler ActivateItemEvent;
        public bool IsInitialized { get; private set; }
        private PrefabsData prefabsData;
        public delegate void AnimationHandler();
        public event AnimationHandler CellAnimationEvent;
        private bool canInteractive;

        internal async void SetInteractive(bool v)
        {
            await Task.Delay(100);
            canInteractive = v;


            MInventoryInput.SetInteractive(v);
            inventoryDrawer.GetSupportContainer().gameObject.SetActive(v);


        }
        private void Awake() => prefabsData = new PrefabsData();


        private void OnEnable()
        {
            inventorySoundEffects = new InventorySoundEffects(MInventoryInput);
            playerStatements = FindObjectOfType<PlayerClasses.PlayerStatements>();
            inventoryDrawer = FindObjectOfType<InventoryDrawer>();

            EventReceiver.OnEnable();
            StartCoroutine(nameof(CellAnimator));
        }

        private void Start()
        {
            // добавление всех ячеек в список

            var sc = inventoryDrawer.GetSupportContainer().GetComponentsInChildren<InventoryCell>();

            foreach (var cell in sc)
            {
                Cells.Add(cell);
                HotCells.Add(cell);
            }
            var mc = inventoryDrawer.GetMainContainer().GetComponentsInChildren<InventoryCell>();
            foreach (var cell in mc)
            {
                Cells.Add(cell);
            }
            foreach (var c in Cells)
            {
                c.Init(this);
            }
            inventorySaver.Load(ref Cells);// загрузка сохранённого инвентаря            

            foreach (var c in Cells)
            {
                CellsRect.Add(c.GetComponent<RectTransform>());
                TakeItemEvent?.Invoke(c.Id, c.Count);
            }

            //загрузка слотов для сундуков
            for (int i = 0; i < ItemsContainer.maxCells; i++)
            {
                Instantiate(cellPrefab, freeCellsContainer);
            }

            IsInitialized = true;

            EventReceiver.SetSMGData(FindObjectOfType<SMG.SMGModifiersData>());
        }

        /// <summary>
        /// добавление поднятого предмета в очередь
        /// </summary>
        /// <param name="item"></param>    
        public void AddItem(int id, int count, SMGInventoryCellGun gun, bool isRecursion = false)
        {
            if (Cells.FindAll(c => c.IsEmpty()).Count == 0)// если не нашлись свободные слоты
            {
                //выбрасывание предмета обратно
                MInventoryInput.DropItem(GetItemPrefab(id), id, count, gun);
                return;
            }

            // поиск слота, с предметом того же типа, и не заполненным   
            var cell = Cells.Find(c => !c.IsFilled && c.Id.Equals(id));

            if (cell == null) cell = Cells.Find(c => c.IsEmpty());// если слот не нашёлся то запись в пустой слот

            cell.SetItem(id, count, gun, false);


            int outOfRange = cell.Count - ItemStates.GetMaxCount(cell.Id);
            if (outOfRange > 0)
            {
                cell.DelItem(outOfRange);
                AddItem(id, outOfRange, gun, true);
            }

            if (!isRecursion)
                TakeItemEvent?.Invoke(id, count);
        }
        public void SpendOnCell() => inventorySoundEffects.PlaySpendClip();
        internal void ClearInventory()
        {
            foreach (var c in Cells)
                c.Clear();
        }

        public void ActivateItem() => EventReceiver.ActivateItem();

        public void CallItemEvent(int id, int count) => ActivateItemEvent?.Invoke(id, count);
        public void MealPlayer(int food, int water) => playerStatements.MealPlayer(food, water);
        internal void Heal((float health, float radiation) medical) => playerStatements.HealPlayer(medical.health, medical.radiation);

        private void OnDisable()
        {
            Save(Cells);
            EventReceiver.OnDisable();
            inventorySoundEffects.OnDisable();
            StopCoroutine(nameof(CellAnimator));
        }
        /// <summary>
        /// сохранение инвентаря
        /// </summary>
        /// <param name="cells"></param>
        private void Save(List<InventoryCell> cells) => inventorySaver.Save(cells);

        public InventoryItem GetItemPrefab(int id) => prefabsData.GetItemPrefab(id);

        private System.Collections.IEnumerator CellAnimator()
        {
            while (true)
            {
                CellAnimationEvent?.Invoke();
                yield return null;
            }
        }
        public sealed class InventorySoundEffects
        {
            private readonly AudioClip spendOnCellClip;// звук при наведении на слот

            private readonly AudioClip openInventoryClip;
            private readonly AudioClip closeInventoryClip;

            private readonly AudioSource inventorySpeaker;
            private readonly InventoryInput inventoryInput;

            private bool isInitialize;
            public InventorySoundEffects(InventoryInput input)
            {
                spendOnCellClip = Resources.Load<AudioClip>("Inventory\\tic_2");
                closeInventoryClip = Resources.Load<AudioClip>("Inventory\\CloseInventory");
                openInventoryClip = Resources.Load<AudioClip>("Inventory\\OpenInventory");

                inventorySpeaker = new GameObject(nameof(inventorySpeaker)).AddComponent<AudioSource>();

                inventorySpeaker.transform.SetParent(GameObject.Find("Common").transform);


                inventoryInput = input;
                inventoryInput.ChangeActiveEvent += PlayEnableClip;
            }

            public void PlaySpendClip() => inventorySpeaker.PlayOneShot(spendOnCellClip);

            private void PlayEnableClip(bool value)
            {
                if (!isInitialize)
                {
                    isInitialize = true;
                    return;
                }
                inventorySpeaker.PlayOneShot(value ? openInventoryClip : closeInventoryClip);
            }

            public void OnDisable() => inventoryInput.ChangeActiveEvent -= PlayEnableClip;
        }
    }
}