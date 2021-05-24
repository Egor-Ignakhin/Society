using System.Collections.Generic;
using UnityEngine;
using static SMG.SMGModifierCharacteristics;

namespace SMG
{
    public class SMGModifiersData : MonoBehaviour
    {
        private static readonly List<(GunTitles modifiableGun, ModifierTypes type, ModifierIndex index)> data =
            new List<(GunTitles modifiableGun, ModifierTypes type, ModifierIndex index)>();

        public List<(GunTitles modifiableGun, ModifierTypes type, ModifierIndex index)> GetModifiersData() => data;
        internal void AddModifier(GunTitles modifiableGun, ModifierTypes type, ModifierIndex index)
        {
            data.Add((modifiableGun, type, index));            
        }
    }
}