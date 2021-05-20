using UnityEngine;
using Inventory;


//объект с возможностью положить в инвентарь
public class InventoryItem : InteractiveObject
{
    [SerializeField] protected int count = 1;
    private InventoryContainer inventoryContainer;    
    [SerializeField] private ItemStates.ItemsID startItem;
    public int Id { get; protected set; }
    protected virtual void Start()
    {        
        inventoryContainer = FindObjectOfType<InventoryContainer>();
        MainDescription = Localization.MainTypes.Item;

        SetId((int)startItem);
        SetType(startItem.ToString());     
    }
    public override void Interact(PlayerClasses.PlayerStatements pl)
    {
        inventoryContainer.AddItem(Id, GetCount());
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
