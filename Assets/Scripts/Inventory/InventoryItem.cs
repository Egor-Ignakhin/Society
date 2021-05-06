using UnityEngine;


//объект с возможностью положить в инвентарь
public class InventoryItem : InteractiveObject
{
    [SerializeField] protected int count = 1;
    private Inventory.InventoryContainer inventoryContainer;
    [SerializeField] private int startid;
    public int Id { get; protected set; }
    protected virtual void Start()
    {
        inventoryContainer = FindObjectOfType<Inventory.InventoryContainer>();
        MainDescription = Localization.MainTypes.Item;

        SetId(startid);
        if (startid == 5)        
            SetType(nameof(Inventory.NameItems.CannedFood));                    
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
