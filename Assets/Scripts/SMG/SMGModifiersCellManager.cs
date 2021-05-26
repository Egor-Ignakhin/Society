using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMG
{
    class SMGModifiersCellManager : MonoBehaviour
    {
        [SerializeField] private RectTransform selectedModifier;
        [SerializeField] private SMGMain main;
        private SMGEventReceiver eventReceiver;
        private void OnEnable()
        {
            eventReceiver = main.EventReceiver;
            eventReceiver.ChangeModfierCell += OnChangeModfierCell;
        }
        private void OnChangeModfierCell(SMGModifiersCell cell)
        {
            cell.transform.SetAsFirstSibling();
            selectedModifier.SetParent(cell.transform);
            selectedModifier.localPosition = Vector3.zero;
            selectedModifier.localPosition += new Vector3(-cell.GetComponent<RectTransform>().sizeDelta.x/4, cell.GetComponent<RectTransform>().sizeDelta.y / 4, 0);
        }
        private void OnDisable()
        {
            if (eventReceiver != null)
                eventReceiver.ChangeModfierCell -= OnChangeModfierCell;
        }
    }
}