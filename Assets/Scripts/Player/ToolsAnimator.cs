using Society.Effects.MapOfWorldCanvasEffects;
using Society.Inventory;

using System.Collections.Generic;

using UnityEngine;

internal class ToolsAnimator : MonoBehaviour
{
    private readonly Dictionary<int, ToolExample> Tools = new Dictionary<int, ToolExample>();
    private int currentActiveToolIt = -1;
    private MapOfWorldCanvas mapOfWorldCanvas;
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var toolEx = transform.GetChild(i).GetComponent<ToolExample>();
            Tools.Add(toolEx.GetID(), toolEx);
        }

        InventoryEventReceiver.ChangeSelectedCellEvent += ChangeTool;
        DisableTools();
        mapOfWorldCanvas = FindObjectOfType<MapOfWorldCanvas>();
    }
    private void ChangeTool(int id)
    {
        if (ItemStates.ItsGun(id))
            id = -1;

        currentActiveToolIt = id;
        DisableTools();
    }
    private void DisableTools(ItemStates.ItemsID itemId = ItemStates.ItemsID.Default)
    {
        foreach (var t in Tools)
        {
            t.Value.gameObject.SetActive((t.Value.GetID() == currentActiveToolIt) && (((ItemStates.ItemsID)t.Value.GetID()) != itemId));
        }
    }
    private void OnDisable()
    {
        InventoryEventReceiver.ChangeSelectedCellEvent -= ChangeTool;
    }

    internal void DisableBinocularsHUD()
    {
        DisableTools();
        mapOfWorldCanvas.EnableAllWithoutBunocule();
    }

    internal void EnableBinocularsHUD()
    {
        DisableTools(ItemStates.ItemsID.Binoculars);
        mapOfWorldCanvas.DisableAllWithoutBunocule();
    }
}
