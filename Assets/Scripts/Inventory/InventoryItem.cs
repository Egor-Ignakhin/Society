using Society.Patterns;

using System.Collections.Generic;

using UnityEngine;

using static Society.SMG.ModifierCharacteristics;
namespace Society.Inventory
{
    /// <summary>
    /// Объект с возможностью положить в инвентарь
    /// </summary>
    public class InventoryItem : InteractiveObject
    {
        [SerializeField] private int count = 1;
        private InventoryContainer inventoryContainer;
        [SerializeField] private ItemStates.ItemsID startItem;
        public int Id { get; private set; }
        [HideInInspector] [SerializeField] private bool itsGun;

        private SMGInventoryCellGun possibleGun = new SMGInventoryCellGun();

        [Space(5)]

        [ShowIf(nameof(itsGun), true)]
        [SerializeField]
        private ModifierIndex magIndex = ModifierIndex.None;
#if UNITY_EDITOR
        [ShowIf(nameof(itsGun), true)] [ReadOnlyField] [SerializeField] private string magVolume;
#endif

        [Space(5)]

        [ShowIf(nameof(itsGun), true)]
        [SerializeField]
        private ModifierIndex silencerIndex = ModifierIndex.None;

        [Space(5)]

        [ShowIf(nameof(itsGun), true)]
        [SerializeField]
        private ModifierIndex aimIndex = ModifierIndex.None;

        [Space(5)]

        [ShowIf(nameof(itsGun), true)]
        [SerializeField]
        private List<GameObject> mags = new List<GameObject>();

        [ShowIf(nameof(itsGun), true)]
        [SerializeField]
        private List<GameObject> silencers = new List<GameObject>();

        [ShowIf(nameof(itsGun), true)]
        [SerializeField] private List<GameObject> aims = new List<GameObject>();

        [Space(5)]

        [ShowIf(nameof(itsGun), true)] [SerializeField] private int ammoCount = 0;

        [ShowIf(nameof(itsGun), true)] [SerializeField] private string ammoType = "Default";
        private bool isDroppedGun = false;

        internal void OnInit(int count, ItemStates.ItemsID item, bool itsGun)
        {
            this.count = count;
            startItem = item;
            this.itsGun = itsGun;
        }

        private void Start()
        {
            inventoryContainer = FindObjectOfType<InventoryContainer>();
            MainDescription = Localization.LocalizationManager.MainTypes.Item;

            SetId((int)startItem);
            SetType(startItem.ToString());
            if (!isDroppedGun)
                possibleGun.Reload(GetGunIdFromItemId(), (int)magIndex, (int)silencerIndex, ammoCount, (int)aimIndex, ammoType);
        }
        public void SetGun(SMGInventoryCellGun g)
        {
            if (g.Title != 0)
                isDroppedGun = true;
            possibleGun.Reload(g);

            magIndex = (ModifierIndex)possibleGun.Mag;
            aimIndex = (ModifierIndex)possibleGun.Aim;
            silencerIndex = (ModifierIndex)possibleGun.Silencer;
            ammoType = possibleGun.AmmoType;
            OnValidate();
        }
        public override void Interact()
        {
            inventoryContainer.AddItem(Id, GetCount(), possibleGun);
            Destroy(gameObject);
        }
        public int GetCount() => count;
        public SMGInventoryCellGun GetPossibleGun() => possibleGun;
        public void SetId(int id)
        {
            this.Id = id;
            SetDescription();
        }
        private void OnValidate()
        {
            itsGun = ItemStates.ItsGun((int)startItem);


            if (!itsGun)
                return;

            SetEnableActiveMod(silencers, (int)silencerIndex);
            SetEnableActiveMod(aims, (int)aimIndex);
            SetEnableActiveMod(mags, (int)magIndex);

#if UNITY_EDITOR
            magVolume = GetAmmoCountFromDispenser(GetGunIdFromItemId(), (int)magIndex).ToString();
#endif
        }
        private void SetEnableActiveMod(List<GameObject> mods, int index)
        {
            for (int i = 0; i < mods.Count; i++)
                mods[i].SetActive(i == index);
        }

        internal void SetCount(int c) => count = c;

        ///Функция преобразующая тип <see cref="ItemStates.ItemsID"/> в тип <see cref="ItemStates.GunsID"> для индекса данного предмета.
        private int GetGunIdFromItemId()
        {
            if (itsGun)
            {
                string nameSI = startItem.ToString();
                int retV = (int)System.Enum.Parse(typeof(ItemStates.GunsID), nameSI) + 1;

                return retV;
            }
            return Id;
        }
    }
}