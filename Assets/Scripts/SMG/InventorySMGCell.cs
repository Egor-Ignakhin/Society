using UnityEngine;
using UnityEngine.UI;
using static SMG.ModifierCharacteristics;

namespace SMG
{/// <summary>
/// класс - слот с модификацией для оружия
/// </summary>
    public class InventorySMGCell : MonoBehaviour, ICellable
    {
        private Image mImage;
        public void OnInit() => mImage = GetComponent<Image>();

        public void Clear() => mImage.sprite = null;

        public void RewriteSprite(SMGTitleTypeIndex modState) => mImage.sprite = GetSprite(modState);
    }
}