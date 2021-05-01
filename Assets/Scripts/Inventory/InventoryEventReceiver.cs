using UnityEngine;
namespace Inventory
{
    /// <summary>
    /// класс - обработчик событий инвентаря
    /// </summary>
    public sealed class InventoryEventReceiver
    {
        public InventoryEventReceiver(Transform mp, FirstPersonController fps)
        {
            mainParent = mp;
            Instance = this;
            this.fps = fps;
        }
        public static InventoryEventReceiver Instance { get; private set; }
        private readonly Transform mainParent;
        private readonly FirstPersonController fps;
        private Transform candidateForReplaceItem;// кандидат для смены местами в инветаре(предмет)
        private InventoryCell candidateForReplaceCell;// кандидат для смены местами в инветаре(ячейка)
        private InventoryCell draggedCell;// удерживаемая ячейка
        private RectTransform draggedItem;// удерживаемый предмет
        private bool isDragged;// происходит ли удержание
        public void OnEnable()
        {
            InventoryInput.ChangeActiveEvent += ChangeActiveEvent;
            InventoryInput.InputKeyEvent += SelectCell;
        }
        public void OnDisable()
        {
            InventoryInput.ChangeActiveEvent -= ChangeActiveEvent;
            InventoryInput.InputKeyEvent -= SelectCell;
        }
        private void ChangeActiveEvent(bool isSimular) => SetPause(InventoryDrawer.Instance.ChangeActiveMainField(isSimular));

        private void SetPause(bool mfEnabled)
        {
            // пауза при открытии инвентаря
            bool enabled = mfEnabled && !Shoots.GunAnimator.Instance.IsAiming;// открытие инвентаря возможно при нажатии на клавишу и не при прицеливании


            Cursor.visible = enabled;
            if (!enabled)
            {
                Cursor.lockState = CursorLockMode.Locked;
                fps.SetState(State.unlocked);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                fps.SetState(State.locked);
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
            if (candidateForReplaceItem != null && candidateForReplaceItem != draggedItem)
            {
                var bufferingSelectItemParent = draggedCell.transform;

                draggedItem.SetParent(candidateForReplaceItem.parent);
                candidateForReplaceItem.SetParent(bufferingSelectItemParent);

                candidateForReplaceItem.localPosition = draggedItem.localPosition = Vector3.zero;

                InventoryCell.CopyPasteCell candidateCopy = new InventoryCell.CopyPasteCell(candidateForReplaceCell);
                InventoryCell.CopyPasteCell draggedCopy = new InventoryCell.CopyPasteCell(draggedCell);

                candidateForReplaceCell.SetItem(draggedCopy);

                draggedCell.SetItem(candidateCopy);
                return;
            }
            if (!draggedCell)
                return;

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
        }
        /// <summary>
        /// снятие выделения со слотов
        /// </summary>
        private void UnfocusAllCells()
        {
            var cells = InventoryContainer.Instance.Cells;
            foreach (var cell in cells)
            {
                cell.SetFocus(false);
            }
        }
        /// <summary>
        /// выделение по нажатию на клавишу
        /// </summary>
        /// <param name="c"></param>
        private void SelectCell(int c)
        {
            if (c > 0 && c <= InventoryContainer.Instance.HotCells.Count)
                FocusCell(InventoryContainer.Instance.HotCells[c - 1]);
        }
    }
}