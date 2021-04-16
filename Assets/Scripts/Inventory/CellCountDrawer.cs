using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Inventory
{
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
            mText.SetText(count != 1 ? count.ToString() : string.Empty);
        }
    }
}