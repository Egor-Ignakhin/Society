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
        [SerializeField] private TMPro.TextMeshProUGUI mText;// счётчик предметов
        public Item MItemContainer { get; private set; }


        public delegate void ItemHandler(int count);
        public event ItemHandler ChangeCountEvent;

        public void Init()
        {
            MItemContainer = new Item(mText);
        }

        /// <summary>
        /// вызывается для изначальной записи предмета в ячейку
        /// </summary>
        /// <param name="item"></param>
        public void SetItem(InventoryItem item)
        {
            MItemContainer.SetItem(item.GetObjectType(), item.GetCount() + MItemContainer.Count);
            ChangeSprite(MItemContainer.Type);

            ChangeCountEvent?.Invoke(MItemContainer.Count);
            IsEmpty = false;
        }

        /// <summary>
        /// вызывается для смены предмета другим предметом
        /// </summary>
        /// <param name="cell"></param>
        public void SetItem(CopyPasteCell copyPaste)
        {
            int outRangeCount = MItemContainer.SetItem(copyPaste.Type, copyPaste.count + MItemContainer.Count);//запись в свободную ячейку кол-во и возвращение излишка

            mItem = copyPaste.mItem;// присвоение новых транс-ов
            mImage = copyPaste.mImage;// и новых image            
            copyPaste.mCell.MItemContainer.SetItem(MItemContainer.Type, outRangeCount);// затем обратный вызов 

            ChangeCountEvent?.Invoke(MItemContainer.Count);
            IsEmpty = false;
        }

        /// <summary>
        /// смена изображения на картинке (в зависимости от типа предмета)
        /// </summary>
        /// <param name="type"></param>
        public void ChangeSprite(string type)
        {
            mImage.sprite = InventorySpriteContainer.GetSprite(type);
            mImage.color = type == InventorySpriteContainer.NameSprites.DefaultIcon ? new Color(1, 1, 1, 0) : Color.white;
        }
        #region moveEvents
        /// <summary>
        /// вызывается при входе курсором в пространство ячейки
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData) => InventoryEventReceiver.Instance.InsideCursorCell(this);

        /// <summary>
        /// вызывается при выходе курсора из пространства ячейки
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData) => InventoryEventReceiver.Instance.OutsideCursorCell();


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
        public RectTransform GetItemTransform() => mItem;

        public Image GetImage() => mImage;

        public class Item
        {
            public string Type { get; private set; } = InventorySpriteContainer.NameSprites.DefaultIcon;
            public int Count { get; private set; } = 0;
            public int MaxCount { get; private set; } = 10;
            public TMPro.TextMeshProUGUI MText { get; private set; }
            public int SetItem(string t, int c)
            {
                MaxCount = ItemStates.GetMaxCount(Type = t);

                Count = c;
               int outRange = (Count) - MaxCount;
                if (Count > MaxCount)
                    Count = MaxCount;

                ReDraw();
                return outRange;
            }
            private void ReDraw()
            {
                MText.SetText(/*Count > 1 ? */Count.ToString() /*: string.Empty*/);// если кол-во > 1 то пишется число предметов
            }
            public Item(TMPro.TextMeshProUGUI t)
            {
                MText = t;
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
                Type = c.MItemContainer.Type;
                count = c.MItemContainer.Count;
                mText = c.MItemContainer.MText;
                mCell = c;
            }
        }
    }
}