using UnityEngine;
using UnityEngine.EventSystems;

namespace Society.Menu
{
    internal class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private MenuManager menuManager;
        private TMPro.TextMeshProUGUI text;
        private Color selectColor = new Color(0.75f, 0.75f, 0.75f);
        private void Awake()
        {
            menuManager = FindObjectOfType<MenuManager>();
            transform.GetChild(0).TryGetComponent(out text);
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