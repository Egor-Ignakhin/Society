using UnityEngine;
using Inventory;
using System.Collections.Generic;


//объект с возможностью положить в инвентарь
public sealed class InventoryItem : InteractiveObject
{
    [SerializeField] private int count = 1;
    private InventoryContainer inventoryContainer;
    [SerializeField] private ItemStates.ItemsID startItem;
    public int Id { get; private set; }
    [SerializeField] private bool itsGun;

    private SMGInventoryCellGun possibleGun = new SMGInventoryCellGun();
    [ShowIf(nameof(itsGun), true)] [SerializeField] private SMG.ModifierCharacteristics.GunTitles titleGun = SMG.ModifierCharacteristics.GunTitles.None;

    [ShowIf(nameof(itsGun), true)]
    [SerializeField]
    private SMG.ModifierCharacteristics.ModifierIndex magIndex = SMG.ModifierCharacteristics.ModifierIndex.None;

    [ShowIf(nameof(itsGun), true)]
    [SerializeField] private List<GameObject> silencers = new List<GameObject>();
    [ShowIf(nameof(itsGun), true)]
    [SerializeField]
    private SMG.ModifierCharacteristics.ModifierIndex silencerIndex = SMG.ModifierCharacteristics.ModifierIndex.None;

    [ShowIf(nameof(itsGun), true)]
    [SerializeField] private List<GameObject> aims = new List<GameObject>();

    [ShowIf(nameof(itsGun), true)]
    [SerializeField]
    private SMG.ModifierCharacteristics.ModifierIndex aimIndex = SMG.ModifierCharacteristics.ModifierIndex.None;

    [EditorButton(nameof(UpdateModifiers), "Update modifiers", activityType: ButtonActivityType.Everything)]
    [ShowIf(nameof(itsGun), true)] [SerializeField] int ammoCount = 0;
    private bool isDroppedGun = false;

    private void Start()
    {
        inventoryContainer = FindObjectOfType<InventoryContainer>();
        MainDescription = Localization.MainTypes.Item;

        SetId((int)startItem);
        SetType(startItem.ToString());
        if (!isDroppedGun)
            possibleGun.Reload((int)titleGun, (int)magIndex, (int)silencerIndex, ammoCount, (int)aimIndex);
    }
    public void SetGun(SMGInventoryCellGun g)
    {
        if (g.Title != 0)
            isDroppedGun = true;
        possibleGun.Reload(g);

        aimIndex = (SMG.ModifierCharacteristics.ModifierIndex)possibleGun.Aim;
        silencerIndex = (SMG.ModifierCharacteristics.ModifierIndex)possibleGun.Silencer;
        UpdateModifiers();
    }
    public override void Interact(PlayerClasses.PlayerStatements pl)
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
    private void UpdateModifiers()
    {
        for (int i = 0; i < aims.Count; i++)
            aims[i].SetActive(i == (int)aimIndex);

        for (int i = 0; i < silencers.Count; i++)
            silencers[i].SetActive(i == (int)silencerIndex);
    }

    internal void SetCount(int c) => count = c;
}
