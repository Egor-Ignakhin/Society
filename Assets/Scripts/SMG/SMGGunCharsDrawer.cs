using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SMG
{
    public class SMGGunCharsDrawer : MonoBehaviour
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

            eventReceiver.ChangeGunModsEvent+= OnChangeSelectedGun;            
        }
        public void OnChangeSelectedGun(Inventory.InventoryCell cell)
        {            
            int id = cell.Id;
            if (!Inventory.ItemStates.ItsGun(id))
                return;
            var chars = GunCharacteristics.GetGunCharacteristics(id);
            titleTextDrawing.text = chars.title;
            titleTextPreview.text = chars.title;
            damageText.text = $"Урон: {chars.damage}";
            maxFlyDistText.text = $"Максимальная дистанция поражения: {chars.maxFlyD}";
            optFlyDistText.text = $"Оптимальная дистанция поражения: {chars.OptFlyD}";
            caliberText.text = $"Калибр: {chars.Caliber}";
            dispVolText.text = $"Объём магазина: {SMGModifierCharacteristics.GetAmmoCountFromDispenser(cell.mSMGGun.Title, cell.mSMGGun.Dispenser)}";
        }
        private void OnDisable()
        {
            if (eventReceiver != null)
            {
                eventReceiver.ChangeGunModsEvent -= OnChangeSelectedGun;                
            }
        }
    }
}