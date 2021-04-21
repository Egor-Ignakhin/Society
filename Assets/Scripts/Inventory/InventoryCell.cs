using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{/// <summary>
/// класс - слота в инвентаре
/// </summary>
    public sealed class InventoryCell : MonoBehaviour, IPointerEnterHandler,
        IPointerExitHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        public bool IsEmpty { get; private set; } = true;// пуст ли слот

        [SerializeField] private Image mImage;// картинка

        [SerializeField] private RectTransform mItem;// трансформация предмета
        public Item MItemContainer { get; private set; } = new Item();


        private Vector3 SelectSize;// обычный размер
        private Vector3 defaultSize;// анимированный размер   

        public delegate void ItemHandler(int count);
        public event ItemHandler ChangeCountEvent;

        public void Init()
        {
            defaultSize = transform.localScale;
            SelectSize = transform.localScale /* 1.1f*/;
        }

        /// <summary>
        /// вызывается для изначальной записи предмета в ячейку
        /// </summary>
        /// <param name="item"></param>
        public void SetItem(InventoryItem item)
        {
            MItemContainer.SetItem(item.GetObjectType(), item.GetCount());
            ChangeSprite(MItemContainer.GetItemType());
            ChangeCountEvent?.Invoke(MItemContainer.GetItemCount());

            IsEmpty = false;
        }

        /// <summary>
        /// вызывается для смены предмета другим предметом
        /// </summary>
        /// <param name="cell"></param>
        public void SetItem(CopyPasteCell copyPaste)
        {
            int outRangeCount = MItemContainer.SetItem(copyPaste.Type, copyPaste.count);
            ChangeCountEvent?.Invoke(MItemContainer.GetItemCount());
            mItem = copyPaste.mItem;
            mImage = copyPaste.mImage;

            copyPaste.mCell.MItemContainer.SetItem(copyPaste.mCell.MItemContainer.GetItemType(), outRangeCount);

            IsEmpty = false;
        }

        /// <summary>
        /// смена изображения на картинке (в зависимости от типа предмета)
        /// </summary>
        /// <param name="type"></param>
        public void ChangeSprite(string type)
        {
            mImage.sprite = InventorySpriteContainer.GetSprite(type);
            mImage.color = type == InventorySpriteContainer.NameSprites.DefaultIcon? new Color(1, 1, 1, 0) : Color.white;
        }
        #region moveEvents
        /// <summary>
        /// вызывается при входе курсором в пространство ячейки
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            InventoryEventReceiver.Instance.InsideCursorCell(this);
            transform.localScale = SelectSize;
        }
        /// <summary>
        /// вызывается при выходе курсора из пространства ячейки
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            InventoryEventReceiver.Instance.OutsideCursorCell();
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

            InventoryEventReceiver.Instance.BeginDrag(this);
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

            InventoryEventReceiver.Instance.DragCell(eventData);
        }

        /// <summary>
        /// вызывается при отмене удержания
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            InventoryEventReceiver.Instance.EndDrag();
            mImage.raycastTarget = true;//возврат чувствительности предмету
        }
        #endregion

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
        public class Item
        {
            private string Type = InventorySpriteContainer.NameSprites.DefaultIcon;
            private int count = 0;
            public int MaxCount { get; private set; } = 10;
            public TMPro.TextMeshProUGUI mText { get; private set; }
            public int SetItem(string t, int c)
            {
                Type = t;
                count += c;
                int outRangeCount = count - MaxCount;
                if (count > MaxCount)
                    count = MaxCount;
                if (outRangeCount > 0)
                    return outRangeCount;
                else return 0;
            }
            public string GetItemType() => Type;

            public int GetItemCount() => count;

            public void SetMaxCount(int mc)
            {
                MaxCount = mc;
            }
            public void SetText(TMPro.TextMeshProUGUI t)
            {
                mText = t;
            }
        }
        /// <summary>
        /// структура для копирования слота
        /// </summary>
        public struct CopyPasteCell
        {
            public InventoryCell mCell;
            public RectTransform mItem;
            public Image mImage;
            public TMPro.TextMeshProUGUI mText;
            public string Type;
            public int count;

            public CopyPasteCell(InventoryCell c)
            {
                mItem = c.GetItemTransform();
                mImage = c.GetImage();
                Type = c.MItemContainer.GetItemType();
                count = c.MItemContainer.GetItemCount();
                mText = c.MItemContainer.mText;
                mCell = c;
            }
        }
    }
}