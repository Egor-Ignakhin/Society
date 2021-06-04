using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SMG.ModifierCharacteristics;

namespace SMG
{/// <summary>
/// класс - слот с модификацией
/// </summary>
    public class SMGModifiersCell : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler, ICellable
    {
        private SMGEventReceiver eventReceiver;
        private Image mImage;
        private Sprite defSprite;
        public Inventory.InventoryCell Ic { get; set; }
        /// <summary>
        /// название оружия - тип модификации - индекс модификации
        /// </summary>
        public SMGTitleTypeIndex TTI { get; private set; }
        private bool IsEmpty => TTI.Title == GunTitles.None;

        public void OnInit(SMGEventReceiver ev, Sprite dfs)
        {
            eventReceiver = ev;
            mImage = transform.GetChild(0).GetComponent<Image>();
            defSprite = dfs;
        }
        public void ChangeModifier(SMGTitleTypeIndex modState, Inventory.InventoryCell icell)
        {
            if (!(mImage.sprite = GetSprite(TTI = modState)))
                Clear();
            Ic = icell;
        }

        public void Clear()
        {
            mImage.sprite = defSprite;
            TTI = SMGTitleTypeIndex.None;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsEmpty)
                return;
            eventReceiver.OnSelectModifierCell(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsEmpty)
                return;
            eventReceiver.OnEnterModifiersCell(this);
        }

        public void OnPointerExit(PointerEventData eventData) => eventReceiver.OnDeselectModifiersCell();
    }
}