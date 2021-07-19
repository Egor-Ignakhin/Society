﻿using UnityEngine;
using UnityEngine.UI;

namespace CampSystem
{
    sealed class RadialSelection : MonoBehaviour
    {
        [SerializeField] private Color baseColor;
        [SerializeField] private Color selectedColor;
        private Image selectionImage;
        private void Start()
        {
            selectionImage = transform.GetChild(0).GetComponent<Image>();
            Deselected();
        }
        public void Selected() => selectionImage.color = selectedColor;

        public void Deselected() => selectionImage.color = baseColor;
    }
}