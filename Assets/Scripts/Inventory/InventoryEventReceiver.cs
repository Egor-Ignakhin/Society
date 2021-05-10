using System.Collections.Generic;
using UnityEngine;
namespace Inventory
{
    /// <summary>
    /// класс - обработчик событий инвентаря
    /// </summary>
    public sealed class InventoryEventReceiver : IGameScreen
    {
        private readonly InventoryContainer inventoryContainer;
        private readonly InventoryInput inventoryInput;
        public InventoryEventReceiver(Transform mp, FirstPersonController fps, Transform freeCellsContainer, Transform busyCellsContainer, 
            InventoryContainer ic, GameObject itemsLabelDescription, InventoryInput input)
        {
            mainParent = mp;
            Instance = this;
            this.fps = fps;
            this.freeCellsContainer = freeCellsContainer;
            this.busyCellsContainer = busyCellsContainer;
            inventoryContainer = ic;
            this.ItemsLabelDescription = itemsLabelDescription;
            inventoryInput = input;
        }
        public static InventoryEventReceiver Instance { get; private set; }
        private readonly Transform mainParent;
        private readonly FirstPersonController fps;
        private readonly Transform busyCellsContainer;
        private readonly Transform freeCellsContainer;
        private readonly GameObject ItemsLabelDescription;
        private ItemsContainer lastItemContainer;
        private int SelectedCellIterator = 1;
        public delegate void ChangeSelectedCell(int id);
        public static event ChangeSelectedCell ChangeSelectedCellEvent;

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
        }
        public void OnDisable()
        {
            inventoryInput.ChangeActiveEvent -= ChangeActiveEvent;
            inventoryInput.InputKeyEvent -= SelectCell;
            inventoryInput.DropEvent -= DropEventReceiver;
            inventoryInput.SpinEvent -= SpinReceiver;
        }
        private void ChangeActiveEvent(bool value) => SetPause(InventoryDrawer.Instance.ChangeActiveMainField(value));

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
            if (isDragged)
                UnfocusAllCells();
            //конец удержания
            isDragged = false;

            ParentingDraggedObject();

            draggedCell = null;
            draggedItem = null;
            candidateForReplaceCell = null;
            candidateForReplaceItem = null;
        }
        private void ParentingDraggedObject()
        {
            // смена местами с кандидатом на ячейку или возврат на место
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
                return;
            }
            if (!draggedCell)
                return;

            //если игрок хочет выкинуть предмет
            if (!IsIntersected(draggedItem.position))
            {
                DropItem(draggedCell.MItemContainer.Id, draggedCell.MItemContainer.Count);
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
            UnfocusAllCells();
            ic.SetFocus(true);
            SelectedCell = ic;
            ItemsLabelDescription.SetActive(true);
        }
        /// <summary>
        /// снятие выделения со слотов
        /// </summary>
        public void UnfocusAllCells()
        {
            var cells = inventoryContainer.Cells;
            foreach (var cell in cells)
            {
                cell.SetFocus(false);
            }
            ItemsLabelDescription.SetActive(false);
            ChangeSelectedCellEvent?.Invoke(0);
        }
        /// <summary>
        /// выделение по нажатию на клавишу
        /// </summary>
        /// <param name="c"></param>
        private void SelectCell(int c)
        {            
            if (c > 0 && c <= inventoryContainer.HotCells.Count)
            {                
                FocusCell(inventoryContainer.HotCells[c-1]);
                ChangeSelectedCellEvent?.Invoke(inventoryContainer.HotCells[c - 1].MItemContainer.Id);
            }
        }

        private void DropItem(int id, int count)
        {
            inventoryInput.DropItem(inventoryContainer.GetItemPrefab(id), count);
        }
        private bool IsIntersected(Vector2 obj)// переделать с проверки расстояния на проверку по пересеч. фигуры (динамической)
        {
            foreach (var c in inventoryContainer.Cells)
            {
                if (Vector2.Distance(obj, c.GetComponent<RectTransform>().position) < 100)
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
            SelectedCell.Clear();
        }
        public void ActivateItem()
        {
            if (SelectedCell)
                SelectedCell.Activate();
        }
        private void SpinReceiver(bool forward)
        {
            if (!SelectedCell)
                return;
           

            if (forward)
                SelectedCellIterator--;
            else
                SelectedCellIterator++;

            if (SelectedCellIterator > inventoryContainer.HotCells.Count)
                SelectedCellIterator = 1;
            if (SelectedCellIterator < 1)
                SelectedCellIterator = inventoryContainer.HotCells.Count;

            SelectCell(SelectedCellIterator);
        }
    }
}