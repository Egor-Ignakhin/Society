using UnityEngine;
using Inventory;


//объект с возможностью положить в инвентарь
public sealed class InventoryItem : InteractiveObject
{
    [SerializeField] private int count = 1;
    private InventoryContainer inventoryContainer;
    [SerializeField] private ItemStates.ItemsID startItem;
    public int Id { get; private set; }
    [SerializeField] private bool itsGun;

    private SMGGunAk_74 possibleGun = new SMGGunAk_74();
    [ShowIf(nameof(itsGun), true)] [SerializeField] private SMG.ModifierCharacteristics.GunTitles titleGun = SMG.ModifierCharacteristics.GunTitles.None;
    [ShowIf(nameof(itsGun), true)] [SerializeField] private SMG.ModifierCharacteristics.ModifierIndex magIndex = SMG.ModifierCharacteristics.ModifierIndex.None;
    [ShowIf(nameof(itsGun), true)] [SerializeField] private SMG.ModifierCharacteristics.ModifierIndex silencerIndex = SMG.ModifierCharacteristics.ModifierIndex.None;
    [ShowIf(nameof(itsGun), true)] [SerializeField] int ammoCount = 0;
    private bool isDroppedGun = false;
    private void Start()
    {
        inventoryContainer = FindObjectOfType<InventoryContainer>();
        MainDescription = Localization.MainTypes.Item;

        SetId((int)startItem);
        SetType(startItem.ToString());
        if (!isDroppedGun)
            possibleGun.Reload((int)titleGun, (int)magIndex, (int)silencerIndex, ammoCount);
    }
    public void SetGun(SMGGunAk_74 g)
    {
        if (g.Title != 0)
            isDroppedGun = true;        
        possibleGun.Reload(g);
    }
    public override void Interact(PlayerClasses.PlayerStatements pl)
    {
        inventoryContainer.AddItem(Id, GetCount(), possibleGun);
        Destroy(gameObject);
    }
    public int GetCount() => count;
    public SMGGunAk_74 GetPossibleGun() => possibleGun;
    public void SetId(int id)
    {
        this.Id = id;
        SetDescription();
    }

    internal void SetCount(int c) => count = c;
}
