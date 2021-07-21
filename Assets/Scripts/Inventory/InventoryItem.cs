using UnityEngine;
using Inventory;
using System.Collections.Generic;
using static SMG.ModifierCharacteristics;
using System;


//объект с возможностью положить в инвентарь
public sealed class InventoryItem : InteractiveObject
{
    [SerializeField] private int count = 1;
    private InventoryContainer inventoryContainer;
    [SerializeField] private ItemStates.ItemsID startItem;
    public int Id { get; private set; }
    [SerializeField] private bool itsGun;

    private SMGInventoryCellGun possibleGun = new SMGInventoryCellGun();
    [ShowIf(nameof(itsGun), true)] [SerializeField] private GunTitles titleGun = GunTitles.None;

    [ShowIf(nameof(itsGun), true)]
    [SerializeField] private List<GameObject> mags = new List<GameObject>();
    [ShowIf(nameof(itsGun), true)]
    [SerializeField]
    private ModifierIndex magIndex = ModifierIndex.None;

    [ShowIf(nameof(itsGun), true)]
    [SerializeField] private List<GameObject> silencers = new List<GameObject>();
    [ShowIf(nameof(itsGun), true)]
    [SerializeField]
    private ModifierIndex silencerIndex = ModifierIndex.None;

    [ShowIf(nameof(itsGun), true)]
    [SerializeField] private List<GameObject> aims = new List<GameObject>();

    [ShowIf(nameof(itsGun), true)]
    [SerializeField]
    private ModifierIndex aimIndex = ModifierIndex.None;
    
    internal void OnInit(int count, ItemStates.ItemsID item, bool itsGun)
    {
        this.count = count;
        startItem = item;
        this.itsGun = itsGun;
    }

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

        gameObject.AddComponent<Effects.InvItemCollision>().OnInit(this, GetComponent<Rigidbody>());
    }
    public void SetGun(SMGInventoryCellGun g)
    {
        if (g.Title != 0)
            isDroppedGun = true;
        possibleGun.Reload(g);

        magIndex = (ModifierIndex)possibleGun.Mag;
        aimIndex = (ModifierIndex)possibleGun.Aim;
        silencerIndex = (ModifierIndex)possibleGun.Silencer;
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
        SetEnableActiveMod(silencers, (int)silencerIndex);
        SetEnableActiveMod(aims, (int)aimIndex);
        SetEnableActiveMod(mags, (int)magIndex);
    }
    private void SetEnableActiveMod(List<GameObject> mods, int index)
    {
        for (int i = 0; i < mods.Count; i++)
            mods[i].SetActive(i == (int)index);
    }

    internal void SetCount(int c) => count = c;
}
