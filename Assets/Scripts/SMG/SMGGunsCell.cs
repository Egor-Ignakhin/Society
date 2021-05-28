using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SMG
{
    public class SMGGunsCell : MonoBehaviour, IPointerClickHandler, ISMGCell
    {
        private SMGEventReceiver eventReceiver;
        public UnityEngine.UI.Image MImage { get; private set; }
        public int Id { get; private set; }        
        public Inventory.InventoryCell Ic { get; set; }

        public void ChangeItem(int id, Inventory.InventoryCell gc)
        {
            Ic = gc;
            MImage.sprite = Inventory.InventorySpriteData.GetSprite(id);
            MImage.color = MImage.sprite ? Color.white : new Color(1, 1, 1, 0.1f);
            Id = id;
        }


        public void OnInit(SMGEventReceiver ev)
        {
            eventReceiver = ev;
            MImage = GetComponent<UnityEngine.UI.Image>();
        }

        public void OnPointerClick(PointerEventData eventData) => eventReceiver.OnSelectGunsCell(this);

        internal void SetMag(ModifierCharacteristics.ModifierIndex index)
        {
            Ic.MGun.SetMag(index);
        }
    }
}