using UnityEngine;
using PlayerClasses;

namespace SMG
{
    /// <summary>
    /// навешивается на модификацию, которую можно подобрать в реальном мире
    /// </summary>
    class GunModifier : InteractiveObject
    {
        [SerializeField] private ModifierCharacteristics.GunTitles modifiableGun;
        [HideIf(nameof(modifiableGun), 0)] [SerializeField] private ModifierCharacteristics.ModifierTypes type;
        [HideIf(nameof(type), 0)] [SerializeField] private ModifierCharacteristics.ModifierIndex index;

        private SMGModifiersData data;
        private void Start()
        {
            data = FindObjectOfType<SMGModifiersData>();
            SetType($"{modifiableGun}_{type}{index}");
        }
        public override void Interact()
        {
            data.AddModifier(new ModifierCharacteristics.SMGTitleTypeIndex(modifiableGun, type, index));
            Destroy(gameObject);
        }
    }
}