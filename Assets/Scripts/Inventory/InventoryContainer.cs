using System.Collections.Generic;
using UnityEngine;

public sealed class InventoryContainer : Singleton<InventoryContainer>
{
    private FirstPersonController fps;
    private Queue<InventoryItem> queueOfItems = new Queue<InventoryItem>();// очередь предметов на отображение
    [SerializeField] private PickUpAndDropDrawer PUDD;// отображетль поднятых п-тов


    private readonly List<InventoryCell> cells = new List<InventoryCell>();//слоты инвентаря
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
        if (cells.FindAll(c => c.isEmpty).Count == 0)// если нашлись свободные слоты
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
        cells.Find(c => c.isEmpty).SetItem(item);
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

    public void InsideCursorCell(InventoryCell cell)
    {
        Debug.Log(cell);
    }
    public void OutsideCursorCell(InventoryCell cell)
    {
        Debug.Log(cell);
    }
}
