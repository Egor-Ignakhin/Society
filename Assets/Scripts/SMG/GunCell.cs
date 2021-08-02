using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SMG
{
    /// <summary>
    /// слот под оружие на верстаке
    /// </summary>
    public class GunCell : MonoBehaviour, IPointerClickHandler, ICellable
    {
        private SMGEventReceiver eventReceiver;
        public UnityEngine.UI.Image MImage { get; private set; }
        public int Id { get; private set; }
        public Inventory.InventoryCell Ic { get; private set; }
        private TMPro.TextMeshProUGUI mText;
        public SMGInventoryCellGun MGun => Ic.MGun;
        private Inventory.InventoryEventReceiver inventoryEventReceiver;

        public bool IsEmpty()
        {
            return Id == 0;
        }
        public void ChangeItem(int id, Inventory.InventoryCell gc)
        {
            Ic = gc;
            MImage.sprite = inventoryEventReceiver.SpriteData.GetSprite(id);
            MImage.color = Color.white;
            Id = id;
            mText.SetText(((Inventory.ItemStates.ItemsID)id).ToString());
        }

        public void Clear()
        {
            Ic = null;
            MImage.sprite = null;
            MImage.color = new Color(1, 1, 1, 0.1f);
            Id = 0;
            mText.SetText(string.Empty);
        }

        public void OnInit(SMGEventReceiver ev)
        {
            eventReceiver = ev;
            MImage = transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
            mText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
            inventoryEventReceiver = FindObjectOfType<Inventory.InventoryContainer>().EventReceiver;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsEmpty())
                return;
            eventReceiver.OnClickGunsCell(this);
        }

        internal void SetMag(ModifierCharacteristics.ModifierIndex index) => MGun.SetMag(index);
        public void SetAim(ModifierCharacteristics.ModifierIndex index) => MGun.SetAim(index);

        internal void SetSilencer(ModifierCharacteristics.ModifierIndex index) => MGun.SetSilencer(index);

        internal void SetModeFromReplacedMode(ModifierCharacteristics.ModifierTypes type, ModifierCharacteristics.ModifierIndex index)
        {
            if (type == ModifierCharacteristics.ModifierTypes.Mag)
                SetMag(index);
            else if (type == ModifierCharacteristics.ModifierTypes.Aim)
                SetAim(index);
            else if (type == ModifierCharacteristics.ModifierTypes.Silencer)
                SetSilencer(index);
        }
    }
}