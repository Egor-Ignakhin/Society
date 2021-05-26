using Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SMG.SMGModifierCharacteristics;

namespace SMG
{
    public class SMGModifiersCell : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
    {
        private SMGEventReceiver eventReceiver;
        private Image mImage;
        public SMGTitleTypeIndex mTTI { get; private set; }
        public bool IsEmpty => mTTI.Title == GunTitles.None;        

        public void OnInit(SMGEventReceiver ev)
        {
            eventReceiver = ev;
            mImage = GetComponent<Image>();
        }
        public void ChangeItem(SMGTitleTypeIndex modState)
        {            
            mImage.sprite = GetSprite(mTTI = modState);
        }
        public void Clear() => mImage.sprite = null;

        public void OnPointerClick(PointerEventData eventData) => eventReceiver.OnActivateCurrentModifierCell(this);

        public void OnPointerEnter(PointerEventData eventData) => eventReceiver.OnSelectModifiersCell(this);

        public void OnPointerExit(PointerEventData eventData) => eventReceiver.OnDeselectModifiersCell();
    }
}