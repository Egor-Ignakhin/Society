using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMG
{
    public class SMGModifiersCellManager : MonoBehaviour
    {
        [SerializeField] private RectTransform selectedModifier;
        [SerializeField] private SMGMain main;
        private SMGEventReceiver eventReceiver;
        private void OnEnable()
        {
            eventReceiver = main.EventReceiver;
            if(eventReceiver != null)
            eventReceiver.ChangeModfierCell += OnChangeModfierCell;
        }
        private void OnChangeModfierCell(SMGModifiersCell cell)
        {
            selectedModifier.SetParent(cell.transform);
            selectedModifier.localPosition = Vector3.zero;
            selectedModifier.localPosition += new Vector3(-24, 24, 0);
        }
        private void OnDisable()
        {
            if (eventReceiver != null)
                eventReceiver.ChangeModfierCell -= OnChangeModfierCell;
        }
    }
}