using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using PlayerClasses;

namespace SMG
{
    class GunModifier : InteractiveObject
    {
        [SerializeField] private ModifierCharacteristics.GunTitles modifiableGun;
        [HideIf(nameof(modifiableGun), 0)] [SerializeField] private ModifierCharacteristics.ModifierTypes type;
        [HideIf(nameof(type), 0)] [SerializeField] private ModifierCharacteristics.ModifierIndex index;

        private SMGModifiersData data;
        private void Start()
        {
            data = FindObjectOfType<SMGModifiersData>();
            SetType("GunModifier");
        }
        public override void Interact(PlayerStatements pl)
        {
            data.AddModifier(new ModifierCharacteristics.SMGTitleTypeIndex(modifiableGun, type, index));
            Destroy(gameObject);
        }
    }
}