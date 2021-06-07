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

        public void ChangeItem(int id, Inventory.InventoryCell gc)
        {
            Ic = gc;
            MImage.sprite = Inventory.InventorySpriteData.GetSprite(id);
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
            MImage = GetComponent<UnityEngine.UI.Image>();
            mText = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Id == 0)
                return;
            eventReceiver.OnSelectGunsCell(this);
        }

        internal void SetMag(ModifierCharacteristics.ModifierIndex index) => Ic.MGun.SetMag(index);
    }
}