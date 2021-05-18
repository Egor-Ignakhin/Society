using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class SMG2DModifier : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image mImage;

    private Color selectedColor = new Color(1, 0.92f, 0.016f, 0.375f);
    private Color unselectedColor = new Color(1, 1, 1, 0.375f);
    private void Awake()
    {
        mImage = GetComponent<Image>();
        SetColorFormSelected(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetColorFormSelected(true);
    }

    private void SetColorFormSelected(bool v)
    {
        mImage.color = v ? selectedColor : unselectedColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SetColorFormSelected(false);
    }
}
