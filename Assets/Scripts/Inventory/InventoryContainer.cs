using System.Collections.Generic;
using UnityEngine;

public sealed class InventoryContainer : Singleton<InventoryContainer>
{
    private FirstPersonController fps;
    private Queue<InventoryItem> queueOfItems = new Queue<InventoryItem>();// очередь предметов на отображение
    [SerializeField] private PickUpAndDropDrawer PUDD;// отображетль поднятых п-тов
    [SerializeField] private Transform mainParent;// родитель для отрисовки поверх всего

    private readonly List<InventoryCell> cells = new List<InventoryCell>();//слоты инвентаря

    private InventoryCell draggedCell;// удерживаемая ячейка
    private RectTransform draggedItem;// удерживаемый предмет
    private bool isDragged;// происходит ли удержание

    private Transform candidateForReplaceItem;// кандидат для смены местами в инветаре(предмет)
    private InventoryCell candidateForReplaceCell;// кандидат для смены местами в инветаре(ячейка)
    private void Awake()
    {
        fps = FindObjectOfType<FirstPersonController>();
    }
    private void OnEnable()
    {
        InventoryInput.ChangeActiveEvent += this.ChangeActiveEvent;
    }
    private void Start()
    {
        // добавление всех ячеек в список
        var mc = InventoryDrawer.Instance.GetMainContainer();
        foreach (var cell in mc.GetComponentsInChildren<InventoryCell>())
        {
            cells.Add(cell);
        }
        var sc = InventoryDrawer.Instance.GetSupportContainer();
        foreach (var cell in sc.GetComponentsInChildren<InventoryCell>())
        {
            cells.Add(cell);
        }
        foreach (var c in cells)
        {
            c.Init();
        }
    }

    /// <summary>
    /// добавление поднятого предмета в очередь
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(InventoryItem item)
    {
        if (cells.FindAll(c => c.IsEmpty).Count == 0)// если нашлись свободные слоты
            return;
        AddNewItem(item);// добавить новый предмет
        queueOfItems.Enqueue(item);// добавить предмет в очередь
        MessageToPUDD();
    }
    private void MessageToPUDD()
    {
        var peek = queueOfItems.Dequeue();
        PUDD.DrawNewItem(peek.GetObjectType(), peek.GetCount());
    }
    private void ChangeActiveEvent()
    {
        InventoryDrawer.ChangeActiveMainField();
        SetPause();
    }

    private void AddNewItem(InventoryItem item)
    {
        cells.Find(c => c.IsEmpty).SetItem(item);
    }
    private void OnDisable()
    {
        InventoryInput.ChangeActiveEvent -= this.ChangeActiveEvent;
    }
    private void SetPause()
    {
        // пауза при открытии инвентаря
        bool enabled = InventoryDrawer.MainFieldEnabled;

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

    public void DragCell(UnityEngine.EventSystems.PointerEventData eventData)
    {
        //удержание предмета
        draggedItem.position = eventData.position;
    }

    public void BeginDrag(InventoryCell cell)
    {
        //начало удержания
        SetDragged(true);
        draggedCell = cell;
        draggedItem = cell.GetItemTransform();
        draggedItem.SetParent(mainParent);
    }
    public void EndDrag()
    {
        //конец удержания
        SetDragged(false);

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
            draggedItem.localPosition = Vector3.zero;

            candidateForReplaceItem.SetParent(bufferingSelectItemParent);
            candidateForReplaceItem.localPosition = Vector3.zero;

            InventoryCell.CopyPasteCell copyPaste = new InventoryCell.CopyPasteCell(candidateForReplaceCell);
            candidateForReplaceCell.SetItem(new InventoryCell.CopyPasteCell(draggedCell));

            draggedCell.SetItem(copyPaste);
        }
        else
        {
            if (draggedCell != null)
            {
                draggedItem.SetParent(draggedCell.transform);
                draggedItem.localPosition = Vector3.zero;
            }
        }

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
    private void SetDragged(bool value)
    {
        isDragged = value;
    }
}
