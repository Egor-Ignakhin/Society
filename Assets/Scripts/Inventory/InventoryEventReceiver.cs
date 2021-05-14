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
        public InventoryEventReceiver(Transform mp, FirstPersonController controller, Transform fCC, Transform bCC,
            InventoryContainer ic, GameObject itemsLabelDescription, InventoryInput input, InventoryDrawer iDrawer, TextMeshProUGUI weightText, Button taB)
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
        }

        internal static void LockScrollEvent(bool isActive)
        {
            ScrollEventLocked = isActive;
        }

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

        public void OpenContainer(List<(int id, int count)> content, int countSlots, ItemsContainer it)
        {
            for (int i = 0; i < countSlots; i++)
            {
                var child = freeCellsContainer.GetChild(0);
                child.SetParent(busyCellsContainer);

                if (content != null)
                    child.GetComponent<InventoryCell>().SetItem(content[i].id, content[i].count);
            }
            inventoryInput.EnableInventory();
            lastItemContainer = it;

            GridLayoutGroup gr = busyCellsContainer.GetComponent<GridLayoutGroup>();
            var rtbtn = takeAllButton.GetComponent<RectTransform>();
            rtbtn.sizeDelta = new Vector2((gr.cellSize.x + gr.spacing.x) * gr.constraintCount, rtbtn.sizeDelta.y);
            rtbtn.position = new Vector3(rtbtn.position.x, 110 * (countSlots / gr.constraintCount), 0);
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

            var cells = new List<(int id, int count)>();

            for (int i = 0; i < childs.Count; i++)
            {
                var item = childs[i].GetComponent<InventoryCell>().MItemContainer;
                cells.Add((item.Id, item.Count));
            }
            lastItemContainer.Close(cells);
            lastItemContainer = null;
        }

        private Transform candidateForReplaceItem;// кандидат для смены местами в инветаре(предмет)
        private InventoryCell candidateForReplaceCell;// кандидат для смены местами в инветаре(ячейка)
        private InventoryCell draggedCell;// удерживаемая ячейка
        private RectTransform draggedItem;// удерживаемый предмет
        private bool isDragged;// происходит ли удержание
        private InventoryCell SelectedCell;

        public void OnEnable()
        {
            inventoryInput.ChangeActiveEvent += ChangeActiveEvent;
            inventoryInput.InputKeyEvent += SelectCell;
            inventoryInput.DropEvent += DropEventReceiver;
            inventoryInput.SpinEvent += SpinReceiver;
            ItemsLabelDescription.SetActive(false);
            inventoryContainer.TakeItemEvent += inventoryMassCalculator.AddItem;
            takeAllButton.onClick.AddListener(TakeAllItemsInContainerReceiver);
        }
        public void OnDisable()
        {
            inventoryInput.ChangeActiveEvent -= ChangeActiveEvent;
            inventoryInput.InputKeyEvent -= SelectCell;
            inventoryInput.DropEvent -= DropEventReceiver;
            inventoryInput.SpinEvent -= SpinReceiver;
            inventoryContainer.TakeItemEvent -= inventoryMassCalculator.AddItem;
            takeAllButton.onClick.RemoveListener(TakeAllItemsInContainerReceiver);
        }
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
        public void DragCell(UnityEngine.EventSystems.PointerEventData eventData)
        {
            //удержание предмета
            draggedItem.position = eventData.position;
        }

        public void BeginDrag(InventoryCell cell)
        {
            //начало удержания
            isDragged = true;
            draggedCell = cell;
            draggedItem = cell.GetItemTransform();
            draggedItem.SetParent(mainParent);
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
            if (!draggedCell)
                return;
            if (draggedCell.MItemContainer.IsEmpty)
                return;

            if (candidateForReplaceItem && candidateForReplaceItem != draggedItem)
            {
                var bufferingSelectItemParent = draggedCell.transform;

                draggedItem.SetParent(candidateForReplaceItem.parent);
                candidateForReplaceItem.SetParent(bufferingSelectItemParent);

                candidateForReplaceItem.localPosition = draggedItem.localPosition = Vector3.zero;

                InventoryCell.CopyPasteCell candidateCopy = new InventoryCell.CopyPasteCell(candidateForReplaceCell);
                InventoryCell.CopyPasteCell draggedCopy = new InventoryCell.CopyPasteCell(draggedCell);

                if (candidateCopy.Equals(draggedCopy))
                {
                    draggedCopy.count += candidateCopy.count;
                    candidateCopy.count = candidateForReplaceCell.SetItem(draggedCopy);

                    if (draggedCopy.count > ItemStates.GetMaxCount(draggedCopy.id))
                    {
                        int outOfRange = draggedCopy.count - ItemStates.GetMaxCount(draggedCopy.id);
                        candidateCopy.count += outOfRange;
                        candidateForReplaceCell.DelItem(outOfRange);
                    }
                }
                else
                    candidateForReplaceCell.SetItem(draggedCopy);


                draggedCell.SetItem(candidateCopy);

                //проверка для изменения массы инвентаря
                if (draggedCell.CellIsInventorySon && candidateForReplaceCell.CellIsInventorySon)// мерж объектов только внутри инвентаря
                    return;
                if (draggedCell.CellIsInventorySon && !candidateForReplaceCell.CellIsInventorySon)// мерж объекта из инвентаря в контейнер
                {
                    inventoryMassCalculator.AddItem(draggedCell.MItemContainer.Id, draggedCell.MItemContainer.Count);
                    inventoryMassCalculator.DeleteItem(candidateForReplaceCell.MItemContainer.Id, candidateForReplaceCell.MItemContainer.Count);
                    return;
                }
                if (!draggedCell.CellIsInventorySon && candidateForReplaceCell.CellIsInventorySon)// мерж объекта из контейнера в инвентарь
                {
                    inventoryMassCalculator.AddItem(candidateForReplaceCell.MItemContainer.Id, candidateForReplaceCell.MItemContainer.Count);
                    inventoryMassCalculator.DeleteItem(draggedCell.MItemContainer.Id, draggedCell.MItemContainer.Count);
                    return;
                }
                return;
            }
            //если игрок хочет выкинуть предмет
            if (!IsIntersected(draggedItem.position))
            {
                DropItem(draggedCell.MItemContainer.Id, draggedCell.MItemContainer.Count);
                if (draggedCell.CellIsInventorySon)
                    inventoryMassCalculator.DeleteItem(draggedCell.MItemContainer.Id, draggedCell.MItemContainer.Count);
                draggedCell.Clear();
            }

            draggedItem.SetParent(draggedCell.transform);
            draggedItem.localPosition = Vector3.zero;
        }

        /// <summary>
        /// выделение слота по нажатию на слот
        /// </summary>
        /// <param name="ic"></param>
        public void FocusCell(InventoryCell ic)
        {
            UnfocusSelectedCell();
            ic.SetFocus(true);
            SelectedCell = ic;
            ItemsLabelDescription.SetActive(true);
        }
        /// <summary>
        /// снятие выделения со слотов
        /// </summary>
        public void UnfocusSelectedCell()
        {
            if (SelectedCell)
                SelectedCell.SetFocus(false);
            SelectedCell = null;

            ItemsLabelDescription.SetActive(false);
            ChangeSelectedCellEvent?.Invoke(0);
        }
        /// <summary>
        /// выделение по нажатию на клавишу
        /// </summary>
        /// <param name="c"></param>
        private void SelectCell(int c)
        {
            if (!(c > 0 && c <= inventoryContainer.GetHotCells().Count))
                return;

            FocusCell(inventoryContainer.GetHotCells()[c - 1]);
            ChangeSelectedCellEvent?.Invoke(inventoryContainer.GetHotCells()[c - 1].MItemContainer.Id);
            ActivateItem();
        }

        private void DropItem(int id, int count)
        {
            inventoryInput.DropItem(inventoryContainer.GetItemPrefab(id), count);
        }

        private bool IsIntersected(Vector2 obj)// переделать с проверки расстояния на проверку по пересеч. фигуры (динамической)
        {
            foreach (var c in inventoryContainer.CellsRect)
            {
                if (Vector2.Distance(obj, c.position) < 100)
                    return true;
            }
            return false;
        }
        private void DropEventReceiver(int _)
        {
            if (!SelectedCell)
                return;
            if (SelectedCell.MItemContainer.IsEmpty)
                return;
            DropItem(SelectedCell.MItemContainer.Id, SelectedCell.MItemContainer.Count);
            if (SelectedCell.CellIsInventorySon)
                inventoryMassCalculator.DeleteItem(SelectedCell.MItemContainer.Id, SelectedCell.MItemContainer.Count);

            SelectedCell.Clear();
        }
        public void ActivateItem()
        {
            if (SelectedCell && SelectedCell.MItemContainer.IsEmpty)
                return;

            int id = SelectedCell.MItemContainer.Id;
            int count = SelectedCell.MItemContainer.Count;
            decimal outRangeWeightItem = ItemStates.GetWeightItem(id) * count;
            if (SelectedCell)
            {
                if (SelectedCell.Activate())
                    inventoryContainer.CallItemEvent(id, 1);
            }
            inventoryMassCalculator.DeleteItem(outRangeWeightItem - ItemStates.GetWeightItem(id) * count);
        }
        private void SpinReceiver(bool forward)
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

            SelectCell(SelectedCellIterator);
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
                if (list.FindAll(c => !c.MItemContainer.IsEmpty).Count == 0)// если не нашлись занятые слоты 
                    break;

                listPlace = list.Find(c => !c.MItemContainer.IsEmpty);
                // поиск слота, с предметом того же типа, и не заполненным   
                var place = places.Find(c => !c.MItemContainer.IsFilled && c.MItemContainer.Id.Equals(listPlace.MItemContainer.Id));

                if (place == null)
                    place = places.Find(c => c.MItemContainer.IsEmpty);// если слот не нашёлся то запись в пустой слот
                if (place == null)//если и пустых нет в инвентаре то выход
                    break;

                draggedCell = listPlace;
                draggedItem = draggedCell.GetItemTransform();
                candidateForReplaceCell = place;
                candidateForReplaceItem = candidateForReplaceCell.GetItemTransform();

                EndDrag();
            }
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