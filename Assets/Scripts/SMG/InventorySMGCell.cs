using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SMG.ModifierCharacteristics;

namespace SMG
{/// <summary>
/// класс - слот с модификацией для оружия
/// </summary>
    public class InventorySMGCell : MonoBehaviour, ICellable, IPointerClickHandler
    {
        private SMGInventoryCellsEventReceiver SMGICEV;
        private Image mImage;
        public SMGTitleTypeIndex TTI { get; private set; }
        public void OnInit(SMGInventoryCellsEventReceiver smgicev)
        {
            mImage = GetComponent<Image>();
            SMGICEV = smgicev;
        }

        public void Clear()
        {
            mImage.sprite = null;
            TTI = SMGTitleTypeIndex.None;
        }

        public void RewriteSprite(SMGTitleTypeIndex modState)
        {
            TTI = modState;
            mImage.sprite = GetSprite(TTI);            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
                SMGICEV.OnClick(this);
        }
    }
}