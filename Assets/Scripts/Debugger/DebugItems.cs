using Society.Inventory;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Debugger
{
    /// <summary>
    /// клас для взятия предметов из дебаггера
    /// </summary>
    internal sealed class DebugItems : MonoBehaviour, IDebug
    {
        public bool Active { get; set; }
        GameObject IDebug.gameObject => gameObject;
        private InventoryContainer inventoryContainer;
        [SerializeField] private Transform ItemsContent;

        private void Awake()
        {
            inventoryContainer = FindObjectOfType<InventoryContainer>();

            //Первичное заполнение предметами             
            for (int i = 1; i < System.Enum.GetNames(typeof(ItemStates.ItemsID)).Length; i++)// проход по каждому предмету
            {
                GameObject itemButton = new GameObject($"{(ItemStates.ItemsID)i} Button", typeof(Image), typeof(Button), typeof(InventoryItem));
                itemButton.GetComponent<Image>().sprite = Resources.Load<Sprite>($"InventoryItems\\{(ItemStates.ItemsID)i}");
                itemButton.GetComponent<Button>().onClick.AddListener(() => AddItem(itemButton.GetComponent<InventoryItem>()));
                itemButton.GetComponent<InventoryItem>().OnInit(1, (ItemStates.ItemsID)i, ItemStates.ItsGun(i));
                var itemGun = new SMGInventoryCellGun();
                itemGun.Reload(
                    i - 1,
                    SMG.GunCharacteristics.GetMaxMagFromID(i),
                    SMG.GunCharacteristics.GetMaxSilencerFromID(i),
                    0,
                    SMG.GunCharacteristics.GetMaxAimFromID(i),
                    "Default");

                itemButton.GetComponent<InventoryItem>().SetGun(itemGun);

                itemButton.transform.SetParent(ItemsContent);
            }
        }

        // выдача инвентарю предмета
        public void AddItem(InventoryItem item) => inventoryContainer.AddItem(item.Id, item.GetCount(), item.GetPossibleGun());

        public void Activate()
        {
            Active = true;
            gameObject.SetActive(true);
        }
    }
}