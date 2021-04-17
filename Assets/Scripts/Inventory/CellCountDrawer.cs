using UnityEngine;
namespace Inventory
{/// <summary>
/// класс-отрисовщик кол-ва предметов в слоте
/// </summary>
    class CellCountDrawer : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI mText;
        [SerializeField] private InventoryCell mCell;
        private void Awake()
        {
            mCell.ChangeCountEvent += ReDraw;
            mCell.MItemContainer.SetText(mText);
        }
        private void ReDraw(int count)
        {
            mText.SetText(count != 1 ? count.ToString() : string.Empty);// если кол-во == 1 то пишется пустая строка
        }
    }
}