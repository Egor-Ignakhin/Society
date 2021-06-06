using System;
using UnityEngine;

namespace SMG
{
    class SMGModifiersCellManager : MonoBehaviour
    {
        [SerializeField] private RectTransform selectedModifier;
        [SerializeField] private SMGMain main;
        private SMGEventReceiver eventReceiver;
        [SerializeField] private ScrollBarController scrollBarController;
        private void OnEnable()
        {
            eventReceiver = main.EventReceiver;
            eventReceiver.UpdateModfiersEvent += OnChangeModfierCell;            
        }
        private void OnChangeModfierCell(SMGModifiersCell cell)
        {
            if (cell.TTI.Index == ModifierCharacteristics.ModifierIndex.None)
            {
                selectedModifier.gameObject.SetActive(false);
                return;
            }
            selectedModifier.gameObject.SetActive(true);
            var cellTr = cell.GetComponent<RectTransform>();
            cellTr.SetAsFirstSibling();
            selectedModifier.SetParent(cellTr);
            selectedModifier.localPosition = new Vector3(-cellTr.sizeDelta.x / 4, cellTr.sizeDelta.y / 4, 0);
            ResetScroll();
        }

        private void ResetScroll() => scrollBarController.ResetScroll();


        private void OnDisable()
        {
            if (eventReceiver != null)
            {
                eventReceiver.UpdateModfiersEvent -= OnChangeModfierCell;                
            }
        }
    }
}