using UnityEngine;
namespace SMG
{/// <summary>
/// класс отвечающий за отображение установленных модификаций
/// </summary>
    public class GunModifiersActiveManager : MonoBehaviour
    {
        [SerializeField] private GameObject mag_1;
        private ModifierCharacteristics.ModifierIndex magIndex;
        private void UpdateModifiers()
        {
            if (magIndex != ModifierCharacteristics.ModifierIndex.None)
                mag_1.SetActive(true);
            else
                mag_1.SetActive(false);
        }
        public void SetMag(ModifierCharacteristics.ModifierIndex nMagix)
        {
            magIndex = nMagix;
            UpdateModifiers();
            Debug.Log(1 + transform.root.name);
        }
    }
}