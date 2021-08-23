using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SMG
{
    /// <summary>
    /// локальный обработчик для слотов с модами в инвентаре
    /// </summary>
    public class SMGInventoryCellsEventReceiver : MonoBehaviour
    {
        [SerializeField] private DynamicalElementsAnswer Answer;
        private Inventory.InventoryInput inventoryInput;
        private ModifierCharacteristics.SMGTitleTypeIndex SelectedTTI;
        private SMGModifiersData modifiersData;
        private Inventory.InventoryEventReceiver inventoryEventReceiver;
        private void Start()
        {
            var icon = FindObjectOfType<Inventory.InventoryContainer>();
            inventoryInput = icon.MInventoryInput;
            inventoryEventReceiver = icon.EventReceiver;
            modifiersData = FindObjectOfType<SMGModifiersData>();
        }
        internal void OnClick(InventorySMGCell inventorySMGCell)
        {
            Answer.Show(DropModifier, null, "Выбросить модификатор?");
            SelectedTTI = inventorySMGCell.TTI;
        }
        private void DropModifier()
        {
            inventoryInput.DropModifier(SelectedTTI);
            modifiersData.RemoveModifier(SelectedTTI);
            inventoryEventReceiver.RewriteSMGCells();

        }
    }
}