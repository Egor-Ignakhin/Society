using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class InventoryCell : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public bool IsEmpty { get; private set; } = true;// пуст ли слот

    [SerializeField] private Image mImage;// картинка
    [SerializeField] private RectTransform mItem;// трансформация предмета
    public Item MItemContainer { get; private set; }


    private Vector3 SelectSize;// обычный размер
    private Vector3 defaultSize;// анимированный размер

    private Transform LastParent;// родитель объекта    

    public void Init()
    {
        defaultSize = transform.localScale;
        SelectSize = transform.localScale /* 1.1f*/;
        MItemContainer = new Item();
    }

    /// <summary>
    /// вызывается для изначальной записи предмета в ячейку
    /// </summary>
    /// <param name="item"></param>
    public void SetItem(InventoryItem item)
    {
        MItemContainer.SetType(item.GetObjectType());
        ChangeSprite(MItemContainer.GetItemType());

        IsEmpty = false;
    }

    /// <summary>
    /// вызывается для смены предмета другим предметом
    /// </summary>
    /// <param name="cell"></param>
    public void SetItem(CopyPasteCell copyPaste)
    {
        MItemContainer.SetType(copyPaste.Type);

        mItem = copyPaste.mItem;
        mImage = copyPaste.mImage;

        IsEmpty = false;
    }

    /// <summary>
    /// смена изображения на картинке (в зависимости от типа предмета)
    /// </summary>
    /// <param name="type"></param>
    public void ChangeSprite(string type)
    {
        mImage.sprite = InventorySpriteContainer.GetSprite(type);
    }
    /// <summary>
    /// вызывается при входе курсором в пространство ячейки
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryContainer.Instance.InsideCursorCell(this);
        transform.localScale = SelectSize;
    }
    /// <summary>
    /// вызывается при выходе курсора из пространства ячейки
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryContainer.Instance.OutsideCursorCell();
        transform.localScale = defaultSize;
    }

    /// <summary>
    /// вызывается в начале удержания
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        LastParent = transform.parent;
        InventoryContainer.Instance.BeginDrag(this);
        mImage.raycastTarget = false;//отключение чувствительности предмета
    }

    /// <summary>
    /// вызывается при удержании предмета
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        //кнопка для удержания обязательно должна быть левой

        InventoryContainer.Instance.DragCell(eventData);
    }

    /// <summary>
    /// вызывается при отмене удержания
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        InventoryContainer.Instance.EndDrag();
        mImage.raycastTarget = true;//возврат чувствительности предмету
    }

    /// <summary>
    /// возвращает трансформацию предмета
    /// </summary>
    /// <returns></returns>
    public RectTransform GetItemTransform()
    {
        return mItem;
    }
    public Image GetImage()
    {
        return mImage;
    }

    public Transform GetLastParent()
    {
        return LastParent;
    }
    public class Item
    {
        private string Type  = InventorySpriteContainer.NameSprites.DefaultIcon;
        public void SetType(string t)
        {
            Type = t;
        }
        public string GetItemType()
        {
            return Type;
        }
    }
    public struct CopyPasteCell
    {
        public RectTransform mItem;
        public Image mImage;
        public string Type;

        public CopyPasteCell(InventoryCell c)
        {
            mItem = c.GetItemTransform();
            mImage = c.GetImage();
            Type = c.MItemContainer.GetItemType();
        }
    }
}
