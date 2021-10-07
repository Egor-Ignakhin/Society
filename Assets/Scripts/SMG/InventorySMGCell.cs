using Society.Patterns;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using static Society.SMG.ModifierCharacteristics;

namespace Society.SMG
{/// <summary>
/// класс - слот с модификацией для оружия
/// </summary>
    public class InventorySMGCell : MonoBehaviour, ICellable, IPointerClickHandler
    {
        private SMGInventoryCellsEventReceiver SMGICEV;
        private Image mImage;
        public SMGTitleTypeIndex TTI { get; private set; }
        public bool IsEmpty()
        {
            return TTI.Equals(SMGTitleTypeIndex.None);
        }
        public void OnInit(SMGInventoryCellsEventReceiver SMGicev)
        {
            mImage = GetComponent<Image>();
            SMGICEV = SMGicev;
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
            if (IsEmpty())
                return;

            if (eventData.button == PointerEventData.InputButton.Right)
                SMGICEV.OnClick(this);
        }
    }
}