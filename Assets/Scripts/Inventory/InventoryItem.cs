using UnityEngine;
using Inventory;


//объект с возможностью положить в инвентарь
public class InventoryItem : InteractiveObject
{
    [SerializeField] protected int count = 1;
    private InventoryContainer inventoryContainer;
    [SerializeField] private int startid;
    public int Id { get; protected set; }
    protected virtual void Start()
    {
        inventoryContainer = FindObjectOfType<InventoryContainer>();
        MainDescription = Localization.MainTypes.Item;

        SetId(startid);
        if (startid == 5)
        {
            SetType(ItemStates.CannedFoodType);
        }
        else if (startid == 6)
        {
            SetType(ItemStates.MilkType);
        }
    }
    public override void Interact(PlayerClasses.PlayerStatements pl)
    {
        inventoryContainer.AddItem(this);
        Destroy(gameObject);
    }
    public int GetCount() => count;
    public void SetId(int id)
    {
        this.Id = id;
        SetDescription();
    }

    internal void SetCount(int c) => count = c;
}
