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
        /// <summary>
        /// название оружия - тип модификации - индекс модификации
        /// </summary>
        public SMGTitleTypeIndex TTI { get; private set; }
        public bool IsEmpty => TTI.Title == GunTitles.None;

        public void OnInit(SMGEventReceiver ev)
        {
            eventReceiver = ev;
            mImage = transform.GetChild(0).GetComponent<Image>();
        }
        public void ChangeModifier(SMGTitleTypeIndex modState)
        {
            mImage.sprite = GetSprite(TTI = modState);
            mImage.color = Color.white;
        }

        public void Clear()
        {
            mImage.sprite = null;
            mImage.color = new Color(0, 0, 0, 0);
        }

        public void OnPointerClick(PointerEventData eventData) => eventReceiver.OnActivateCurrentModifierCell(this);

        public void OnPointerEnter(PointerEventData eventData)
        {            
            if (mImage.sprite == null) 
                return;
            eventReceiver.OnEnterModifiersCell(this);
        }

        public void OnPointerExit(PointerEventData eventData) => eventReceiver.OnDeselectModifiersCell();
    }
}