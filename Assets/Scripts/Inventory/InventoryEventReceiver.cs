using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Inventory
{
    /// <summary>
    /// класс - обработчик событий инвентаря
    /// </summary>
    public sealed class InventoryEventReceiver : IGameScreen
    {
        private readonly InventoryContainer inventoryContainer;
        private readonly InventoryInput inventoryInput;
        private static bool ScrollEventLocked = false;

        private readonly Transform mainParent;
        private readonly FirstPersonController fps;
        private readonly Transform busyCellsContainer;
        private readonly Transform freeCellsContainer;
        private readonly GameObject ItemsLabelDescription;
        private ItemsContainer lastItemContainer;
        private int SelectedCellIterator = 0;

        public delegate void ChangeSelectedCell(int id);
        public static event ChangeSelectedCell ChangeSelectedCellEvent;

        private readonly InventoryMassCalculator inventoryMassCalculator;
        private readonly InventoryDrawer inventoryDrawer;
        private readonly Button takeAllButton;

        private Transform candidateForReplaceItem;// кандидат для смены местами в инветаре(предмет)
        private InventoryCell candidateForReplaceCell;// кандидат для смены местами в инветаре(ячейка)
        private InventoryCell draggedCell;// удерживаемая ячейка
        private RectTransform draggedItem;// удерживаемый предмет
        private bool isDragged;// происходит ли удержание
        private InventoryCell SelectedCell;
        private InventoryCell lastSelectedCell;
        private GameObject modifiersPage;
        private Button modPageButton;
        private SMG.SMGModifiersData modifiersData;
        private bool canFastMoveSelCell = false;//можно ли перемещать слоты в инвентаре на быстрый доступ если нажат шифт
        public InventoryEventReceiver(Transform mp, FirstPersonController controller, Transform fCC, Transform bCC,
         InventoryContainer ic, GameObject itemsLabelDescription, InventoryInput input, InventoryDrawer iDrawer,
         TextMeshProUGUI weightText, Button taB, Button modbtn, GameObject modPage)
        {
            mainParent = mp;
            fps = controller;
            freeCellsContainer = fCC;
            busyCellsContainer = bCC;
            inventoryContainer = ic;
            ItemsLabelDescription = itemsLabelDescription;
            inventoryInput = input;
            inventoryMassCalculator = new InventoryMassCalculator(fps, weightText);
            inventoryDrawer = iDrawer;
            takeAllButton = taB;
            modPageButton = modbtn;
            modifiersPage = modPage;
        }
        public void OnEnable()
        {
            inventoryInput.ChangeActiveEvent += ChangeActiveEvent;
            inventoryInput.InputKeyEvent += OnInputCellsNums;
            inventoryInput.DropEvent += OnDropEvent;
            inventoryInput.ScrollEvent += OnScrollEvent;
            ItemsLabelDescription.SetActive(false);
            inventoryContainer.TakeItemEvent += inventoryMassCalculator.AddItem;
            inventoryInput.FastMoveCellEvent += OnInputFastMoveCell;
            takeAllButton.onClick.AddListener(TakeAllItemsInContainerReceiver);
            takeAllButton.gameObject.SetActive(false);
            modifiersPage.SetActive(false);
            modPageButton.onClick.AddListener(ModifiersPageChangeActive);

            foreach (var m in modifiersPage.GetComponentsInChildren<SMG.InventorySMGCell>())
                m.OnInit();
        }
        internal static void LockScrollEvent(bool isActive) => ScrollEventLocked = isActive;
        private void ChangeActiveEvent(bool value) => SetPause(inventoryDrawer.ChangeActiveMainField(value));

        private void SetPause(bool enabled)
        {
            Cursor.visible = enabled;
            if (!enabled)
            {
                Cursor.lockState = CursorLockMode.Locked;
                fps.SetState(State.unlocked);
                if (lastItemContainer != null)
                    CloseContainer();
                ScreensManager.SetScreen(null);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                fps.SetState(State.locked);
                ScreensManager.SetScreen(this);
                RewriteSMGCells();
            }
            EndDrag();
        }
        public void InsideCursorCell(InventoryCell cell)
        {
            // событие входа курсора в сектор ячейки
            candidateForReplaceCell = cell;
            candidateForReplaceItem = cell.GetItemTransform();

            if (isDragged)
                return;
            draggedCell = cell;
            draggedItem = cell.GetItemTransform();
        }
        public InventoryCell GetSelectedCell() => SelectedCell;
        public InventoryCell GetLastSelectedCell() => lastSelectedCell;

        public void OutsideCursorCell()
        {
            // событие выхода курсора из сектора ячейки
            candidateForReplaceItem = null;
            candidateForReplaceCell = null;

            if (isDragged)
                return;
            draggedCell = null;
            draggedItem = null;
        }

        public void BeginDrag(InventoryCell cell)
        {
            //начало удержания
            isDragged = true;
            draggedCell = cell;
            draggedItem = cell.GetItemTransform();
            draggedItem.SetParent(mainParent);
        }

        internal void SetSMGData(SMG.SMGModifiersData sMGModifiersData)
        {
            modifiersData = sMGModifiersData;
        }

        public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
        {
            //удержание предмета
            draggedItem.position = eventData.position;
        }

        public void EndDrag()
        {
            //конец удержания
            isDragged = false;

            ParentingDraggedObject();

            draggedCell = null;
            draggedItem = null;
            candidateForReplaceCell = null;
            candidateForReplaceItem = null;
        }
        /// <summary>
        /// смена местами с кандидатом на ячейку или возврат на место
        /// </summary>
        private void ParentingDraggedObject()
        {
            if (!draggedCell || draggedCell.IsEmpty)
                return;

            if (candidateForReplaceItem && candidateForReplaceItem != draggedItem)
            {
                var bufferingSelectItemParent = draggedCell.transform;

                draggedItem.SetParent(candidateForReplaceItem.parent);
                candidateForReplaceItem.SetParent(bufferingSelectItemParent);

                candidateForReplaceItem.localPosition = draggedItem.localPosition = Vector3.zero;

                InventoryCell.CopyPasteCell candidateCopy = new InventoryCell.CopyPasteCell(candidateForReplaceCell);
                InventoryCell.CopyPasteCell draggedCopy = new InventoryCell.CopyPasteCell(draggedCell);

                if (draggedCopy.Equals(candidateCopy))
                {
                    draggedCopy.Count += candidateCopy.Count;
                    candidateCopy.Count = Math.Abs(candidateCopy.Count + candidateForReplaceCell.SetItem(draggedCopy));
                }
                else
                    candidateForReplaceCell.SetItem(draggedCopy, false);



                draggedCell.SetItem(candidateCopy, false);

                //проверка для изменения массы инвентаря
                if (draggedCell.CellIsInventorySon && candidateForReplaceCell.CellIsInventorySon)// мерж объектов только внутри инвентаря
                    return;
                if (draggedCell.CellIsInventorySon && !candidateForReplaceCell.CellIsInventorySon)// мерж объекта из инвентаря в контейнер
                {
                    inventoryMassCalculator.AddItem(draggedCell.Id, draggedCell.Count);
                    inventoryMassCalculator.DeleteItem(candidateForReplaceCell.Id, candidateForReplaceCell.Count);
                    return;
                }
                if (!draggedCell.CellIsInventorySon && candidateForReplaceCell.CellIsInventorySon)// мерж объекта из контейнера в инвентарь
                {
                    inventoryMassCalculator.AddItem(candidateForReplaceCell.Id, candidateForReplaceCell.Count);
                    inventoryMassCalculator.DeleteItem(draggedCell.Id, draggedCell.Count);
                    return;
                }
                return;
            }
            //если игрок хочет выкинуть предмет
            if (!IsIntersected(draggedItem.position))
            {
                DropItem(draggedCell.Id, draggedCell.Count, draggedCell.mSMGGun);
                if (draggedCell.CellIsInventorySon)
                    inventoryMassCalculator.DeleteItem(draggedCell.Id, draggedCell.Count);
                draggedCell.Clear();
            }

            draggedItem.SetParent(draggedCell.transform);
            draggedItem.localPosition = Vector3.zero;
        }

        /// <summary>
        /// выделение слота по нажатию на слот
        /// </summary>
        /// <param name="ic"></param>
        public void FocusCell(InventoryCell ic, bool isScroll = false)
        {
            if (canFastMoveSelCell)
            {
                if (ic.IsEmpty)
                    return;
                InventoryCell emptyCell;
                if ((emptyCell = inventoryContainer.GetHotCells().Find(c => c.IsEmpty)) && !isScroll)// если нашлись пустые слоты
                {
                    draggedCell = ic;
                    draggedItem = draggedCell.GetItemTransform();
                    candidateForReplaceCell = emptyCell;
                    candidateForReplaceItem = candidateForReplaceCell.GetItemTransform();

                    EndDrag();
                    return;
                }
            }
            lastSelectedCell = SelectedCell;
            UnfocusSelectedCell(SelectedCell);
            ic.SetFocus(true);
            SelectedCell = ic;
            ItemsLabelDescription.SetActive(true);
        }
        /// <summary>
        /// снятие выделения со слотов
        /// </summary>
        public void UnfocusSelectedCell(InventoryCell cell)
        {
            if (SelectedCell && cell == SelectedCell)
            {
                SelectedCell.SetFocus(false);
                SelectedCell = null;
                ItemsLabelDescription.SetActive(false);
                ChangeSelectedCellEvent?.Invoke(0);
            }
        }
        private void OnInputCellsNums(int c)
        {
            if (!(c > 0 && c <= inventoryContainer.GetHotCells().Count))
                return;
            SelectCell(c);
        }
        /// <summary>
        /// выделение по нажатию на клавишу
        /// </summary>
        /// <param name="c"></param>
        private void SelectCell(int c, bool isScroll = false)
        {
            FocusCell(inventoryContainer.GetHotCells()[c - 1], isScroll);
            ChangeSelectedCellEvent?.Invoke(inventoryContainer.GetHotCells()[c - 1].Id);
            if (!isScroll)
                ActivateItem();
        }

        private void DropItem(int id, int count, SMGGunAk_74 gun)
        {
            inventoryInput.DropItem(inventoryContainer.GetItemPrefab(id), count, gun);
        }

        // переделать с проверки расстояния на проверку по пересеч. фигуры (динамической)
        private bool IsIntersected(Vector2 obj) => inventoryContainer.CellsRect.Find(c => Vector2.Distance(obj, c.position) < 100);

        private void OnDropEvent(int _)
        {
            if (!SelectedCell || SelectedCell.IsEmpty)
                return;

            DropItem(SelectedCell.Id, SelectedCell.Count, SelectedCell.mSMGGun);
            if (SelectedCell.CellIsInventorySon)
                inventoryMassCalculator.DeleteItem(SelectedCell.Id, SelectedCell.Count);

            SelectedCell.Clear();
        }
        public void ActivateItem()
        {
            if (SelectedCell && SelectedCell.IsEmpty)
                return;

            int id = SelectedCell.Id;
            int count = SelectedCell.Count;
            decimal outRangeWeightItem = ItemStates.GetWeightItem(id) * count;
            if (SelectedCell)
            {
                if (SelectedCell.Activate())
                    inventoryContainer.CallItemEvent(id, 1);
            }
            inventoryMassCalculator.DeleteItem(outRangeWeightItem - ItemStates.GetWeightItem(id) * count);
        }
        private void OnScrollEvent(bool forward)
        {
            if (ScrollEventLocked)
                return;
            var hotcells = inventoryContainer.GetHotCells();
            if (!SelectedCell)
                SelectedCell = hotcells[0];

            SelectedCellIterator += forward ? -1 : 1;


            if (SelectedCellIterator > hotcells.Count)
                SelectedCellIterator = 1;
            if (SelectedCellIterator < 1)
                SelectedCellIterator = hotcells.Count;

            SelectCell(SelectedCellIterator, true);
        }
        private void TakeAllItemsInContainerReceiver()
        {
            if (!lastItemContainer)
                return;
            var list = busyCellsContainer.GetComponentsInChildren<InventoryCell>().ToList();
            var places = inventoryContainer.GetCells();
            InventoryCell listPlace = null;
            while (true)// нашлись свободные слоты
            {
                if (list.FindAll(c => !c.IsEmpty).Count == 0)// если не нашлись занятые слоты 
                    break;

                listPlace = list.Find(c => !c.IsEmpty);
                // поиск слота, с предметом того же типа, и не заполненным   
                var place = places.Find(c => !c.IsFilled && c.Id.Equals(listPlace.Id));

                if (place == null)
                    place = places.Find(c => c.IsEmpty);// если слот не нашёлся то запись в пустой слот
                if (place == null)//если и пустых нет в инвентаре то выход
                    break;

                draggedCell = listPlace;
                draggedItem = draggedCell.GetItemTransform();
                candidateForReplaceCell = place;
                candidateForReplaceItem = candidateForReplaceCell.GetItemTransform();

                EndDrag();
            }
        }

        public void OpenContainer(List<(int id, int count, SMGGunAk_74 gun)> content, int countSlots, ItemsContainer it)
        {
            for (int i = 0; i < countSlots; i++)
            {
                var child = freeCellsContainer.GetChild(0);
                child.SetParent(busyCellsContainer);

                if (content != null)
                    child.GetComponent<InventoryCell>().SetItem(content[i].id, content[i].count, content[i].gun);
            }
            inventoryInput.EnableInventory();
            lastItemContainer = it;

            GridLayoutGroup gr = busyCellsContainer.GetComponent<GridLayoutGroup>();
            var rtbtn = takeAllButton.GetComponent<RectTransform>();
            rtbtn.sizeDelta = new Vector2((gr.cellSize.x + gr.spacing.x) * gr.constraintCount, rtbtn.sizeDelta.y);
            rtbtn.position = new Vector3(rtbtn.position.x, gr.transform.position.y + (gr.cellSize.y * countSlots / gr.constraintCount), 0);
            takeAllButton.gameObject.SetActive(true);
        }

        internal void DelItem(ItemStates.ItemsID bulletId, int count)
        {
            var cells = inventoryContainer.GetCells().FindAll(c => c.Id == (int)bulletId);
            var foundedCell = cells.OrderBy(c => c.Count).First();
            //var foundedCell = cells.Find(c => c.Id == (int)bulletId/* && c.Count == minCount*/);

            if (foundedCell)
            {
                if (foundedCell.Count >= count)
                {
                    foundedCell.DelItem(count);
                    count = 0;
                }
                else
                {
                    count -= foundedCell.Count;
                    foundedCell.DelItem(foundedCell.Count);
                    DelItem(bulletId, count);
                }
            }
        }

        internal int Containts(ItemStates.ItemsID bulletId)
        {
            int count = 0;

            inventoryContainer.GetCells().FindAll(c => c.Id == (int)bulletId && (count += c.Count) > -1);
            return count;
        }

        public void CloseContainer()
        {
            List<Transform> childs = new List<Transform>();
            for (int i = 0; i < busyCellsContainer.childCount; i++)
            {
                childs.Add(busyCellsContainer.GetChild(i));
            }
            foreach (var c in childs)
            {
                c.SetParent(freeCellsContainer);
            }

            var cells = new List<(int id, int count, SMGGunAk_74 gun)>();

            for (int i = 0; i < childs.Count; i++)
            {
                var item = childs[i].GetComponent<InventoryCell>();
                cells.Add((item.Id, item.Count, item.mSMGGun));
            }
            lastItemContainer.Close(cells);
            lastItemContainer = null;
            takeAllButton.gameObject.SetActive(false);
        }
        private void RewriteSMGCells()
        {
            var smgCells = modifiersPage.GetComponentsInChildren<SMG.InventorySMGCell>();
            var modData = modifiersData.GetModifiersData();
            foreach (var c in smgCells)
                c.Clear();
            for (int i = 0; i < modData.Count; i++)
            {
                smgCells[i].RewriteSprite(modData[i]);
            }
        }
        public void OnDisable()
        {
            inventoryInput.ChangeActiveEvent -= ChangeActiveEvent;
            inventoryInput.InputKeyEvent -= OnInputCellsNums;
            inventoryInput.DropEvent -= OnDropEvent;
            inventoryInput.ScrollEvent -= OnScrollEvent;
            inventoryInput.FastMoveCellEvent -= OnInputFastMoveCell;
            inventoryContainer.TakeItemEvent -= inventoryMassCalculator.AddItem;
            takeAllButton.onClick.RemoveListener(TakeAllItemsInContainerReceiver);
            modPageButton.onClick.RemoveListener(ModifiersPageChangeActive);
        }
        public void ModifiersPageChangeActive()
        {
            modifiersPage.SetActive(!modifiersPage.activeInHierarchy);
        }
        private void OnInputFastMoveCell(bool v)
        {
            canFastMoveSelCell = v;
        }
        public class InventoryMassCalculator
        {
            public decimal Weight { get; private set; } = 0;
            public const decimal MaxWeightForRunningMass = 30;
            private readonly FirstPersonController fps;
            private readonly TextMeshProUGUI weightText;
            public InventoryMassCalculator(FirstPersonController controller, TextMeshProUGUI wT)
            {
                fps = controller;
                weightText = wT;
            }

            public void AddItem(int id, int count)
            {
                Weight += ItemStates.GetWeightItem(id) * count;
                RecalculatePlayerSpeed();
            }
            public void DeleteItem(int id, int count)
            {
                Weight -= ItemStates.GetWeightItem(id) * count;
                RecalculatePlayerSpeed();
            }
            public void DeleteItem(decimal w)
            {
                Weight -= w;
                RecalculatePlayerSpeed();
            }
            public void RecalculatePlayerSpeed()
            {
                decimal playerBraking = 1 - (Weight / MaxWeightForRunningMass / 5);
                if (playerBraking < .8m)// становление самой минимальной скорости
                {
                    playerBraking = .8m;
                    weightText.color = Color.red;
                }
                else weightText.color = Color.white;

                fps.SetBraking((float)playerBraking);
                weightText.SetText($"Вес: {Weight}/{MaxWeightForRunningMass}");
            }
        }
    }
}