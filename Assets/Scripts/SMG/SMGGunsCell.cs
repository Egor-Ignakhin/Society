using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SMGGunsCell : MonoBehaviour, IPointerClickHandler
{
    private SMGEventReceiver eventReceiver;

    public void OnInit(SMGEventReceiver ev)
    {
        eventReceiver = ev;
    }

    public void OnPointerClick(PointerEventData eventData) => eventReceiver.SelectGunsCell(this);

}
