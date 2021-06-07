using UnityEngine;
using TMPro;
namespace SMG
{
    /// <summary>
    /// рисовщик информации о модификаторе
    /// </summary>
    public class SMGModifiersCellDescription : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textDesc;
        [SerializeField] private UnityEngine.UI.Image mImage;

        public void ChangeModifier(ModifierCharacteristics.SMGTitleTypeIndex tti)
        {
            (string title, string description, Sprite sprite) = ("", "", null);
            if (tti.Title != ModifierCharacteristics.GunTitles.None)
            {
                (title, description, sprite) = ModifierCharacteristics.GetTitleDescSprite(tti);
            }
            textTitle.text = title;
            textDesc.text = description;
            mImage.sprite = sprite;
        }
        private void Update() => transform.localPosition = Input.mousePosition + new Vector3(20, 20);
    }
}