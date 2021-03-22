using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryContainer : Singleton <InventoryContainer>
{
    private Queue<InventoryItem> queueOfItems = new Queue<InventoryItem>();
    [SerializeField] private PickUpAndDropDrawer PUDD;

    public void AddItem(InventoryItem item)
    {
        queueOfItems.Enqueue(item);
        MessageToPUDD();
    }
    private void MessageToPUDD()
    {
        var peek = queueOfItems.Dequeue();
        PUDD.DrawNewItem(peek.GetTypeObject(), peek.GetCount());
    }
}
