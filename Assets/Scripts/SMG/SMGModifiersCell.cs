using UnityEngine;
using UnityEngine.EventSystems;

namespace SMG
{
    public class SMGModifiersCell : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
    {
        private SMGEventReceiver eventReceiver;

        public void OnInit(SMGEventReceiver ev)
        {
            eventReceiver = ev;
        }

        public void OnPointerClick(PointerEventData eventData) => eventReceiver.OnActivateCurrentModifierCell(this);

        public void OnPointerEnter(PointerEventData eventData) => eventReceiver.OnSelectModifiersCell(this);



        public void OnPointerExit(PointerEventData eventData) => eventReceiver.OnDeselectModifiersCell();
    }
}