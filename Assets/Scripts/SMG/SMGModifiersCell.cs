using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SMGModifiersCell : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    private SMGEventReceiver eventReceiver;

    public void OnInit(SMGEventReceiver ev)
    {
        eventReceiver = ev;
    }

    public void OnPointerClick(PointerEventData eventData) => eventReceiver.ActivateCurrentModifierCell(this);

    public void OnPointerEnter(PointerEventData eventData) => eventReceiver.SelectModifiersCell(this);



    public void OnPointerExit(PointerEventData eventData) => eventReceiver.DeselectModifiersCell(this);
}
