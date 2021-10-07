using Society.Inventory;

using UnityEngine;

namespace Society.SMG
{
    /// <summary>
    /// локальный обработчик для слотов с модами в инвентаре
    /// </summary>
    public class SMGInventoryCellsEventReceiver : MonoBehaviour
    {
        [SerializeField] private DynamicalElementsAnswer Answer;
        private InventoryInput inventoryInput;
        private ModifierCharacteristics.SMGTitleTypeIndex SelectedTTI;
        private SMGModifiersData modifiersData;
        private InventoryEventReceiver inventoryEventReceiver;
        private void Start()
        {
            var icon = FindObjectOfType<InventoryContainer>();
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