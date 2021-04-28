using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{/// <summary>
/// класс - слота в инвентаре
/// </summary>
    public sealed class InventoryCell : MonoBehaviour, IPointerEnterHandler,
        IPointerExitHandler, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerClickHandler
    {
        [SerializeField] private Image background;
        [SerializeField] private Image mImage;// картинка

        [SerializeField] private RectTransform mItem;// трансформация предмета
        [SerializeField] private TextMeshProUGUI mText;// счётчик предметов
        public Item MItemContainer { get; private set; } = new Item();
        private AdditionalSettins additionalSettins;
        public sealed class AdditionalSettins
        {
            public Vector3 DefaultScale { get; }
            public Vector3 AnimatedScale { get; }
            public Color FocusedColor { get; }
            public Color UnfocusedColor { get; }
            public AdditionalSettins(Image bg)
            {
                DefaultScale = bg.GetComponent<RectTransform>().localScale;
                AnimatedScale = DefaultScale * 1.1f;
                FocusedColor = new Color(0, 1, 0, bg.color.a);
                UnfocusedColor = bg.color;
            }
        }

        public void Init()
        {            
            additionalSettins = new AdditionalSettins(background);
        }

        /// <summary>
        /// вызывается для изначальной записи предмета в ячейку
        /// </summary>
        /// <param name="item"></param>
        public void SetItem(InventoryItem item)
        {
            MItemContainer.SetItem(item.GetObjectType(), item.GetCount());

            ChangeSprite(MItemContainer.Type);
            UpdateText();
        }
        /// <summary>
        /// вызывается для смены предмета другим предметом
        /// </summary>
        /// <param name="cell"></param>
        public int SetItem(CopyPasteCell copyPaste)
        {
            int outRangeCount = MItemContainer.SetItem(copyPaste.Type, copyPaste.count, true);//запись в свободную ячейку кол-во и возвращение излишка

            mItem = copyPaste.mItem;// присвоение новых транс-ов
            mImage = copyPaste.mImage;// и новых image                        
            mText = copyPaste.mText;
            ChangeSprite(MItemContainer.Type);
            UpdateText();
            return outRangeCount;
        }
        private void UpdateText()
        {
            mText.SetText(MItemContainer.Count > 1 ? MItemContainer.Count.ToString() : string.Empty);// если кол-во > 1 то пишется число предметов           
        }

        /// <summary>
        /// смена изображения на картинке (в зависимости от типа предмета)
        /// </summary>
        /// <param name="type"></param>
        public void ChangeSprite(string type)
        {
            mImage.sprite = InventorySpriteContainer.GetSprite(type);
            mImage.color = type == NameItems.DefaultIcon ? new Color(1, 1, 1, 0) : Color.white;
        }
        #region Events
        /// <summary>
        /// вызывается при входе курсором в пространство ячейки
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            InventoryEventReceiver.Instance.InsideCursorCell(this);            
            StartCoroutine(nameof(BackgroundAnimate));
        }

        /// <summary>
        /// вызывается при выходе курсора из пространства ячейки
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            InventoryEventReceiver.Instance.OutsideCursorCell();
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

        /// <summary>
        /// вызывается при нажатии на слот
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            InventoryEventReceiver.Instance.FocusCell(this);
        }
        private IEnumerator BackgroundAnimate()
        {
            bool wasAnimated = false;
            InventoryContainer.Instance.SpendOnCell();
            while (true)
            {
                var rt = background.GetComponent<RectTransform>();
                Vector3 nextState = wasAnimated? additionalSettins.DefaultScale : additionalSettins.AnimatedScale;
                rt.localScale = Vector3.MoveTowards(rt.localScale, nextState, 0.5f);
                if (rt.localScale == additionalSettins.AnimatedScale)
                {
                    wasAnimated = true;
                }
                if (wasAnimated && rt.localScale == additionalSettins.DefaultScale)
                    break;
                yield return null;
            }
        }
        #endregion

        /// <summary>
        /// возвращает трансформацию предмета
        /// </summary>
        /// <returns></returns>
        public RectTransform GetItemTransform() => mItem;

        public Image GetImage() => mImage;
        public void SetFocus(bool v)
        {
            background.color = v ? additionalSettins.FocusedColor : additionalSettins.UnfocusedColor;
        }

        public class Item
        {
            public bool IsFilled { get => Count > MaxCount - 1; }
            public string Type { get; private set; } = NameItems.DefaultIcon;
            public int Count { get; private set; } = 0;
            public int MaxCount { get => ItemStates.GetMaxCount(Type); }
            public bool IsEmpty { get => Count == 0; }
            public int SetItem(string ntype, int ncount, bool isMerge = false)
            {
                int outRange = 0;
                if (ntype == Type && !isMerge)// если тип предмета тот же, что и был в слоте
                {
                    outRange = MaxCount - (Count += ncount);// получаем выход за границу
                    if (Count > MaxCount)
                        Count = MaxCount;
                }
                else// иначе просто замена
                    Count = ncount;

                Type = ntype;
                return outRange;
            }
        }
        /// <summary>
        /// структура для копирования слота
        /// </summary>
        public struct CopyPasteCell
        {
            public TextMeshProUGUI mText;
            public RectTransform mItem;
            public Image mImage;
            public string Type;
            public int count;

            public CopyPasteCell(InventoryCell c)
            {
                mItem = c.GetItemTransform();
                mImage = c.GetImage();
                Type = c.MItemContainer.Type;
                count = c.MItemContainer.Count;
                mText = c.mText;
            }
        }
    }
}