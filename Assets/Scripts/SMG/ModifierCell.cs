using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SMG.ModifierCharacteristics;

namespace SMG
{/// <summary>
/// класс - слот с модификацией
/// </summary>
    public class ModifierCell : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler, ICellable
    {
        private SMGEventReceiver eventReceiver;// обработчик событий СМО
        private Image mImage;
        private Sprite defSprite;// спрайт для пустого слота
        public Inventory.InventoryCell Ic { get; set; }
        /// <summary>
        /// название оружия - тип модификации - индекс модификации
        /// </summary>
        public SMGTitleTypeIndex TTI { get; private set; }
        public bool IsEmpty()
        {
            return TTI.Title == GunTitles.None;
        }

        public void OnInit(SMGEventReceiver ev, Sprite dfs)
        {
            eventReceiver = ev;
            mImage = transform.GetChild(0).GetComponent<Image>();
            defSprite = dfs;
        }
        /// <summary>
        /// запись нового модификатора
        /// </summary>
        /// <param name="modState"></param>
        /// <param name="icell"></param>
        public void ChangeModifier(SMGTitleTypeIndex modState, Inventory.InventoryCell icell)
        {
            if (!(mImage.sprite = GetSprite(TTI = modState)))
                Clear();
            Ic = icell;
        }

        /// <summary>
        /// очистка слота
        /// </summary>
        public void Clear()
        {
            mImage.sprite = defSprite;
            TTI = SMGTitleTypeIndex.None;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsEmpty())
                return;
            eventReceiver.OnSelectModifierCell(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsEmpty())
                return;
            eventReceiver.OnEnterModifiersCell(this);
        }

        public void OnPointerExit(PointerEventData eventData) => eventReceiver.OnDeselectModifiersCell();
    }
}