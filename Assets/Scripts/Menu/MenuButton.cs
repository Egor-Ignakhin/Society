﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace Society.Menu
{
    /// <summary>
    /// Компонент, который анимирует наведение и сведение курсора с текста элемента
    /// </summary>
    internal class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private MenuManager menuManager;
        private TMPro.TextMeshProUGUI text;
        private Color selectColor = new Color(0.75f, 0.75f, 0.75f);
        private void Awake()
        {
            menuManager = FindObjectOfType<MenuManager>();
            TryGetComponent(out text);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            menuManager.OnButtonEnter();
            text.color = selectColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            text.color = Color.white;
        }
    }
}