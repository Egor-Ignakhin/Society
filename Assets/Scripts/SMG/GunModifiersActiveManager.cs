using System.Collections.Generic;
using UnityEngine;
namespace SMG
{/// <summary>
/// класс отвечающий за отображение установленных модификаций
/// </summary>
    public class GunModifiersActiveManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> mags = new List<GameObject>();
        [SerializeField] private ModifierCharacteristics.ModifierIndex magIndex;
        [SerializeField] private List<GameObject> aims = new List<GameObject>();
        [SerializeField] private ModifierCharacteristics.ModifierIndex aimIndex;
        [SerializeField] private List<GameObject> silencers = new List<GameObject>();
        [SerializeField] private ModifierCharacteristics.ModifierIndex silencerIndex;
        private void UpdateModifiers()
        {
            for (int i = 0; i < mags.Count; i++)            
                mags[i].SetActive(i == (int)magIndex);
            
            for (int i = 0; i < aims.Count; i++)            
                aims[i].SetActive(i == (int)aimIndex);

            for (int i = 0; i < silencers.Count; i++)
                silencers[i].SetActive(i == (int)silencerIndex);
        }
        public void SetMag(ModifierCharacteristics.ModifierIndex nMagix)
        {
            magIndex = nMagix;
            UpdateModifiers();
        }
        public void SetAim(ModifierCharacteristics.ModifierIndex aiIndex)
        {
            aimIndex = aiIndex;
            UpdateModifiers();
        }
        public void SetSilencer(ModifierCharacteristics.ModifierIndex sIndex)
        {
            silencerIndex = sIndex;
            UpdateModifiers();
        }
    }
}