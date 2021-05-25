using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SMG
{
    public class SMGElementsSupport : MonoBehaviour
    {
        [SerializeField] private GameObject background;
        [SerializeField] private SMGMain main;

        public bool IsActive => background.activeInHierarchy;

        private void Awake()
        {
            background.SetActive(false);
        }
        public void Show()
        {
            transform.localPosition = Input.mousePosition;
            background.SetActive(true);
        }
        public void Unequip()
        {
            background.SetActive(false);
            main.UnequipGunElement();            
        }
        public void Cancel()
        {
            background.SetActive(false);
            main.DeselectGunElement();
        }
    }
}