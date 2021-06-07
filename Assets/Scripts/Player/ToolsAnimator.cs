using Inventory;
using System.Collections.Generic;
using UnityEngine;

class ToolsAnimator : MonoBehaviour
{
    private readonly Dictionary<int, ToolExample> Tools = new Dictionary<int, ToolExample>();    
    private int currentActiveToolIt = -1;
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var toolEx = transform.GetChild(i).GetComponent<ToolExample>();
            Tools.Add(toolEx.GetID(), toolEx);
        }

        InventoryEventReceiver.ChangeSelectedCellEvent += ChangeTool;
        DisableTools();
    }
    private void ChangeTool(int id)
    {
        if (ItemStates.ItsGun(id))
            id = -1;

        currentActiveToolIt = id;
        DisableTools();
    }
    private void DisableTools()
    {
        foreach(var t in Tools)
        {
            t.Value.gameObject.SetActive(t.Value.GetID() == currentActiveToolIt);
        }
    }
    private void OnDisable()
    {
        InventoryEventReceiver.ChangeSelectedCellEvent -= ChangeTool;
    }
}
