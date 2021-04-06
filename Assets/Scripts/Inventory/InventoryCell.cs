using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class InventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isEmpty { get; private set; } = true;

    [SerializeField]private Image mImage;

    private Vector3 SelectSize;
    private Vector3 defaultSize;

    private void Start()
    {
        defaultSize = transform.localScale;
        SelectSize = transform.localScale * 1.1f;
    }
    public void SetItem(InventoryItem item)
    {
        ChangeSprite(item.GetObjectType());

        isEmpty = false;
    }
    public void ChangeSprite(string type)//new type
    {
        mImage.sprite = InventorySpriteContainer.GetSprite(type);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryContainer.Instance.InsideCursorCell(this);
        transform.localScale = SelectSize;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryContainer.Instance.OutsideCursorCell(this);
        transform.localScale = defaultSize;
    }
}
