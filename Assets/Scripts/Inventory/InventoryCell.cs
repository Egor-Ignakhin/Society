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
        IPointerExitHandler, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerClickHandler, ICellable
    {
        private Image background;// фон картинки
        public Image MImage { get; private set; }// картинка
        public RectTransform MItem { get; private set; }// трансформация предмета
        private TextMeshProUGUI mText;// счётчик предметов
        private Item MItemContainer { get; set; } = new Item();
        private AdditionalSettins additionalSettins;
        private InventoryEventReceiver eventReceiver;
        private InventoryContainer inventoryContainer;
        public bool CellIsInventorySon { get; private set; } = false;// слот инвентарный
        #region fast Access to mContainer
        public int Id => MItemContainer.Id;
        public int Count => MItemContainer.Count;
        public bool IsEmpty()=> MItemContainer.IsEmpty;        
        public bool IsFilled => MItemContainer.IsFilled;
        #endregion
        public SMGInventoryCellGun MGun { get; private set; } = new SMGInventoryCellGun();// контейнер для возможного оружия
        private RectTransform mRt;
        public sealed class AdditionalSettins
        {
            public readonly Vector3 DefaultScale; // обычный размер
            public readonly Vector3 AnimatedScale;// анимированный размер
            public readonly Color FocusedColor;// цвет при выделении
            public readonly Color UnfocusedColor;// обычный цвет
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
            if (additionalSettins is null)
                Init(null);
        }
        public void Init(InventoryContainer ic)
        {
            background = GetComponent<Image>();
            MImage = transform.GetChild(0).GetComponent<Image>();
            MItem = MImage.GetComponent<RectTransform>();
            MItem.GetChild(0).TryGetComponent(out mText);
            mRt = GetComponent<RectTransform>();
            additionalSettins = new AdditionalSettins(background);
            if (!ic)// инициализация проходящая для слотов контейнеров
            {
                inventoryContainer = FindObjectOfType<InventoryContainer>();
            }
            else// инициализация проходящая для слотов инвентаря
            {
                inventoryContainer = ic;
                CellIsInventorySon = true;
                SetAmmoCount(MGun.AmmoCount);
            }
            eventReceiver = inventoryContainer.EventReceiver;
        }

        internal void ReloadGun(SMGInventoryCellGun gun)
        {
            MGun.Reload(gun);
            UpdateText();
        }
        /// <summary>
        /// вызывается для записи предмета в ячейку после загрузки
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <param name="pos"></param>
        public int SetItem(int id, int count, SMGInventoryCellGun gun, bool isMerge = true)
        {
            int outOfRange = MItemContainer.SetItem(id, count + Count, isMerge);
            ReloadGun(gun);
            ChangeSprite();

            return outOfRange;
        }
        /// <summary>
        /// вызывается для смены предмета другим предметом
        /// </summary>
        /// <param name="cell"></param>
        public int SetItem(CopyPasteCell copyPaste, bool isMerge = true)
        {
            int outRangeCount = MItemContainer.SetItem(copyPaste.Id, copyPaste.Count, isMerge);//запись в свободную ячейку кол-во и возвращение излишка

            MItem = copyPaste.MItem;// присвоение новых транс-ов
            MImage = copyPaste.MImage;// и новых image                        
            mText = copyPaste.MText;
            MGun = copyPaste.PossibleGun;

            ChangeSprite();
            ReloadGun(copyPaste.PossibleGun);
            return outRangeCount;
        }
        private void UpdateText()
        {
            // если кол-во > 1 то пишется число предметов                       
            mText.SetText(Count > 1 ? Count.ToString() : (ItemStates.ItsGun(Id) ? MGun.AmmoCount.ToString() : string.Empty));
        }

        public void Clear()
        {
            MItemContainer.DelItem(Count);
            ChangeSprite();
        }
        /// <summary>
        /// смена изображения на картинке (в зависимости от типа предмета)
        /// </summary>
        /// <param name="type"></param>
        public void ChangeSprite()
        {
            MImage.sprite = InventorySpriteData.GetSprite(Id);
            MImage.color = IsEmpty() ? new Color(1, 1, 1, 0) : Color.white;
            UpdateText();
            ///если контейнер пуст
            if (IsEmpty())
                eventReceiver.UnfocusSelectedCell(this);//снимается фокус со слота
        }

        internal void SetAmmoCount(int remBullets)
        {
            MGun.SetAmmoCount(remBullets);
            UpdateText();
        }
        #region Events
        /// <summary>
        /// вызывается при входе курсором в пространство ячейки
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            eventReceiver.InsideCursorCell(this);
            inventoryContainer.CellAnimationEvent += BackgroundAnimate;
            wasAnimated = false;
            inventoryContainer.SpendOnCell();
        }

        /// <summary>
        /// вызывается при выходе курсора из пространства ячейки
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerExit(PointerEventData eventData) => eventReceiver.OutsideCursorCell();



        /// <summary>
        /// вызывается в начале удержания
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (IsEmpty())
                return;

            eventReceiver.BeginDrag(this);
            MImage.raycastTarget = false;//отключение чувствительности предмета            
        }

        /// <summary>
        /// вызывается при удержании предмета
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (IsEmpty())
                return;

            eventReceiver.OnDrag(eventData);
        }

        /// <summary>
        /// вызывается при отмене удержания
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (IsEmpty())
                return;

            eventReceiver.EndDrag();
            MImage.raycastTarget = true;//возврат чувствительности предмету
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

        bool wasAnimated = false;
        private void BackgroundAnimate()
        {
            Vector3 nextState = wasAnimated ? additionalSettins.DefaultScale : additionalSettins.AnimatedScale;
            mRt.localScale = Vector3.MoveTowards(mRt.localScale, nextState, 0.5f);
            if (mRt.localScale == additionalSettins.AnimatedScale)
                wasAnimated = true;

            if (wasAnimated && mRt.localScale == additionalSettins.DefaultScale)
                inventoryContainer.CellAnimationEvent -= BackgroundAnimate;
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
                    Id = (int)ItemStates.ItemsID.Default;
            }
            public int SetItem(int nid, int ncount, bool isMerge = true)
            {
                int outRange = 0;
                if (Id == nid && isMerge)// если тип предмета тот же, что и был в слоте
                {
                    outRange = MaxCount - (Count += ncount);// получаем выход за границу                    
                    Count = Mathf.Clamp(Count, 0, MaxCount);
                }
                else// иначе просто замена
                    Count = ncount;

                Id = IsEmpty ? (int)ItemStates.ItemsID.Default : nid;

                return outRange;
            }
        }
        /// <summary>
        /// структура для копирования слота
        /// </summary>
        public struct CopyPasteCell
        {
            public TextMeshProUGUI MText { get; set; }
            public RectTransform MItem { get; set; }
            public Image MImage { get; set; }
            public int Count { get; set; }
            public int Id { get; set; }
            public SMGInventoryCellGun PossibleGun { get; set; }

            public CopyPasteCell(InventoryCell c)
            {
                MItem = c.MItem;
                MImage = c.MImage;
                Count = c.Count;
                MText = c.mText;
                Id = c.Id;
                PossibleGun = c.MGun;
            }
            public bool Equals(CopyPasteCell obj) => obj.Id == Id && obj.Count < ItemStates.GetMaxCount(Id) && Count < ItemStates.GetMaxCount(Id);

        }

        /// <summary>
        /// метод "активации предмета (например поедание еды)"
        /// </summary>
        public bool Activate()
        {
            if (ItemStates.ItsMeal(Id))
            {
                var meal = ItemStates.GetMeatNutrition(Id);
                inventoryContainer.MealPlayer(meal.Item1, meal.Item2);
                DelItem(1);
                return true;
            }
            else if (ItemStates.ItsMedical(Id))
            {
                var medical = ItemStates.GetMedicalPower(Id);
                inventoryContainer.Heal(medical);
                DelItem(1);
                return true;
            }
            return false;
        }
    }
}