using UnityEngine;
using TMPro;

namespace SMG
{
    /// <summary>
    /// класс - рисовщик хар. об оружии
    /// </summary>
    class SMGGunCharsDrawer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleTextDrawing;
        [SerializeField] private TextMeshProUGUI titleTextPreview;

        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private TextMeshProUGUI maxFlyDistText;
        [SerializeField] private TextMeshProUGUI optFlyDistText;
        [SerializeField] private TextMeshProUGUI caliberText;
        [SerializeField] private TextMeshProUGUI dispVolText;

        private SMGEventReceiver eventReceiver;
        private void OnEnable()
        {
            eventReceiver = FindObjectOfType<SMGMain>().EventReceiver;

            eventReceiver.ChangeSelectedGunEvent += OnChangeSelectedGun;            
        }
        public void OnChangeSelectedGun(Inventory.InventoryCell cell)
        {            
            int id = cell.Id;
            if (!Inventory.ItemStates.ItsGun(id))
                return;            
            var (title, damage, maxFlyD, OptFlyD, Caliber) = GunCharacteristics.GetGunCharacteristics(id);
            titleTextDrawing.text = title;
            titleTextPreview.text = title;
            damageText.text = $"Урон: {damage}";
            maxFlyDistText.text = $"Максимальная дистанция поражения: {maxFlyD}";
            optFlyDistText.text = $"Оптимальная дистанция поражения: {OptFlyD}";
            caliberText.text = $"Калибр: {Caliber}";
            dispVolText.text = $"Объём магазина: {ModifierCharacteristics.GetAmmoCountFromDispenser(cell.MGun.Title, cell.MGun.Dispenser)}";
        }
        private void OnDisable()
        {
            if (eventReceiver != null)            
                eventReceiver.ChangeSelectedGunEvent -= OnChangeSelectedGun;                            
        }
    }
}