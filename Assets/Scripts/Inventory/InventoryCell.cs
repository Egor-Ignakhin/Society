using System;
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
        private InventoryEventReceiver eventReceiver;
        private InventoryContainer inventoryContainer;
        public bool CellIsInventorySon { get; private set; } = false;
        public sealed class AdditionalSettins
        {
            public Vector3 DefaultScale { get; } // обычный размер
            public Vector3 AnimatedScale { get; } // анимированный размер
            public Color FocusedColor { get; }// цвет при выделении
            public Color UnfocusedColor { get; }// обычный цвет
            public AdditionalSettins(Image bg)
            {
                DefaultScale = bg.GetComponent<RectTransform>().localScale;
                AnimatedScale = DefaultScale * 1.1f;
                FocusedColor = new Color(0, 1, 0, bg.color.a);
                UnfocusedColor = bg.color;
            }
        }

        private void Awake()
        {
            if (additionalSettins == null)
                Init(null);
        }
        public void Init(InventoryContainer ic)
        {
            additionalSettins = new AdditionalSettins(background);
            if (!ic)// инициализация проходящая для слотов контейнеров
            {
                inventoryContainer = FindObjectOfType<InventoryContainer>();                
            }
            else// инициализация проходящая для слотов инвентаря
            {
                inventoryContainer = ic;
                CellIsInventorySon = true;
            }
            eventReceiver = inventoryContainer.EventReceiver;           
        }        
        /// <summary>
        /// вызывается для записи предмета в ячейку после загрузки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <param name="pos"></param>
        public void SetItem(int id, int count)
        {
            MItemContainer.SetItem(id, count);
            ChangeSprite();
        }
        /// <summary>
        /// вызывается для смены предмета другим предметом
        /// </summary>
        /// <param name="cell"></param>
        public int SetItem(CopyPasteCell copyPaste)
        {
            int outRangeCount = MItemContainer.SetItem(copyPaste.id, copyPaste.count, true);//запись в свободную ячейку кол-во и возвращение излишка

            mItem = copyPaste.mItem;// присвоение новых транс-ов
            mImage = copyPaste.mImage;// и новых image                        
            mText = copyPaste.mText;

            ChangeSprite();
            mItem.localScale = additionalSettins.DefaultScale;
            return outRangeCount;
        }
        private void UpdateText() => mText.SetText(MItemContainer.Count > 1 ? MItemContainer.Count.ToString() : string.Empty);// если кол-во > 1 то пишется число предметов           

        public void Clear()
        {
            SetItem(0, 0);
        }
        /// <summary>
        /// смена изображения на картинке (в зависимости от типа предмета)
        /// </summary>
        /// <param name="type"></param>
        public void ChangeSprite()
        {
            mImage.sprite = InventorySpriteContainer.GetSprite(MItemContainer.Id);
            mImage.color = MItemContainer.IsEmpty ? new Color(1, 1, 1, 0) : Color.white;
            UpdateText();
            ///если контейнер пуст
            if (MItemContainer.IsEmpty)
                eventReceiver.UnfocusAllCells();//снимается фокус со слота
        }
        #region Events
        /// <summary>
        /// вызывается при входе курсором в пространство ячейки
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            eventReceiver.InsideCursorCell(this);
            StartCoroutine(nameof(BackgroundAnimate));
        }

        /// <summary>
        /// вызывается при выходе курсора из пространства ячейки
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData)
        {
            eventReceiver.OutsideCursorCell();
        }


        /// <summary>
        /// вызывается в начале удержания
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            if (MItemContainer.IsEmpty)
                return;

            eventReceiver.BeginDrag(this);
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

            if (MItemContainer.IsEmpty)
                return;

            eventReceiver.DragCell(eventData);
        }

        /// <summary>
        /// вызывается при отмене удержания
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (MItemContainer.IsEmpty)
                return;

            eventReceiver.EndDrag();
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
            eventReceiver.FocusCell(this);
        }
        private IEnumerator BackgroundAnimate()
        {
            bool wasAnimated = false;
            inventoryContainer.SpendOnCell();
            while (true)
            {
                var rt = background.GetComponent<RectTransform>();
                Vector3 nextState = wasAnimated ? additionalSettins.DefaultScale : additionalSettins.AnimatedScale;
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
        /// удаляет указанное кол-во предметов в слоте
        /// </summary>
        /// <param name="outOfRange"></param>
        internal void DelItem(int outOfRange)
        {
            MItemContainer.DelItem(outOfRange);
            ChangeSprite();
        }

        public RectTransform GetItemTransform() => mItem;
        public Image GetImage() => mImage;
        public void SetFocus(bool v) => background.color = v ? additionalSettins.FocusedColor : additionalSettins.UnfocusedColor;


        public class Item
        {
            public bool IsFilled { get => Count > MaxCount - 1; }
            public int Id { get; set; }
            public int Count { get; private set; } = 0;
            public int MaxCount { get => ItemStates.GetMaxCount(Id); }
            public bool IsEmpty { get => Count == 0; }
            public void DelItem(int count)
            {
                Count -= count;
                if (IsEmpty)
                    Id = NameItems.Default;
            }
            public int SetItem(int nid, int ncount, bool isMerge = false)
            {
                int outRange = 0;
                if (Id == nid && !isMerge)// если тип предмета тот же, что и был в слоте
                {
                    outRange = MaxCount - (Count += ncount);// получаем выход за границу
                    if (Count > MaxCount)
                        Count = MaxCount;
                }
                else// иначе просто замена
                    Count = ncount;

                Id = IsEmpty ? NameItems.Default : nid;

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
            public int count;
            public int id;

            public CopyPasteCell(InventoryCell c)
            {
                mItem = c.GetItemTransform();
                mImage = c.GetImage();
                count = c.MItemContainer.Count;
                mText = c.mText;
                id = c.MItemContainer.Id;
            }
            public bool Equals(CopyPasteCell obj)
            {
                return obj.id == id && obj.count < ItemStates.GetMaxCount(id) && count < ItemStates.GetMaxCount(id);
            }
        }

        /// <summary>
        /// метод "активации предмета (например поедание еды)"
        /// </summary>
        public void Activate()
        {
            if (MItemContainer.Id == 5 || MItemContainer.Id == 6)
            {
                var meal = ItemStates.GetMeatNutrition(MItemContainer.Id);
                inventoryContainer.MealPlayer(meal.Item1, meal.Item2);
                DelItem(1);
            }
        }
    }
}