using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class InventoryContainer : Singleton<InventoryContainer>
{
    private FirstPersonController fps;
    private Queue<InventoryItem> queueOfItems = new Queue<InventoryItem>();// очередь предметов на отображение
    [SerializeField] private PickUpAndDropDrawer PUDD;// отображетль поднятых п-тов


    private readonly List<InventoryCell> cells = new List<InventoryCell>();//слоты инвентаря

    private InventoryCell draggedCell;
    private RectTransform draggedItem;
    private bool isDragged;

    private Transform candidateForReplaceItem;
    private InventoryCell candidateForReplaceCell;
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
        var mc = InventoryDrawer.Instance.GetMainContainer();
        foreach (var cell in mc.GetComponentsInChildren<InventoryCell>())
        {
            AddCell(cell);
        }
        var sc = InventoryDrawer.Instance.GetSupportContainer();
        foreach (var cell in sc.GetComponentsInChildren<InventoryCell>())
        {
            AddCell(cell);
        }
        foreach (var c in cells)
        {
            c.Init();
        }
    }
    private void AddCell(InventoryCell cell)
    {
        cells.Add(cell);
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
    }

    public void DragCell(UnityEngine.EventSystems.PointerEventData eventData)
    {
        draggedItem.position = eventData.position;
    }

    public void BeginDrag(InventoryCell cell)
    {
        SetDragged(true);
        draggedCell = cell;
        draggedItem = cell.GetItemTransform();
    }
    public void EndDrag()
    {
        SetDragged(false);

        if (candidateForReplaceItem != null && candidateForReplaceItem != draggedItem)
        {
            var bufferingSelectItemParent = draggedItem.parent;

            draggedItem.SetParent(candidateForReplaceItem.parent);
            draggedItem.localPosition = Vector3.zero;

            candidateForReplaceItem.SetParent(bufferingSelectItemParent);
            candidateForReplaceItem.localPosition = Vector3.zero;

            RectTransform rr = candidateForReplaceCell.GetItemTransform();
            Image ri = candidateForReplaceCell.GetImage();
            candidateForReplaceCell.SetItem(draggedCell.GetItemTransform(), draggedCell.GetImage());

            draggedCell.SetItem(rr, ri);

            Debug.LogWarning("select = " + draggedCell.GetItemTransform().name);
            Debug.LogWarning("candidate = " + candidateForReplaceCell.GetItemTransform().name);
        }
        else
        {
            draggedItem.localPosition = draggedCell.LastParent.localPosition;
        }

        draggedCell = null;
        draggedItem = null;
        candidateForReplaceCell = null;
        candidateForReplaceItem = null;
    }
    public void InsideCursorCell(InventoryCell cell)
    {
        candidateForReplaceCell = cell;
        candidateForReplaceItem = cell.GetItemTransform();

        if (isDragged)
            return;
        draggedCell = cell;
        draggedItem = cell.GetItemTransform();
    }

    public void OutsideCursorCell()
    {
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
