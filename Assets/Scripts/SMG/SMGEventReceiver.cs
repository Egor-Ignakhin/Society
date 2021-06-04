using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SMG
{

    public class SMGEventReceiver
    {
        private readonly List<SMGModifiersCell> ModifiersCells = new List<SMGModifiersCell>();//слоты с модификаторами
        private readonly List<GunsCell> gunsCells = new List<GunsCell>();// слоты с оружием
        private readonly GameObject modifiersAnswer;// подсказка о модификаторе        
        private SMGModifiersCell currentModCell;//активный слот под модификации
        private GunsCell currentGunCell;
        private readonly Inventory.InventoryContainer inventoryContainer;// главный обработчик инвентаря         
        private readonly SMGCamera MSMGCamera;// камера для отрисовки окна предпросмотра
        private readonly SMGModifiersData modifiersData;// контейнер под модификации
        private readonly SMGModifiersCellDescription modifiersCellDescription;// динамичное описание для модификаций
        public delegate void ChangeCellHandler(SMGModifiersCell t);
        public event ChangeCellHandler ChangeModfierCell;// событие смены активного модификатора
        public delegate void ChangeGunModsHandler(Inventory.InventoryCell ic);
        public event ChangeGunModsHandler ChangeSelectedGunEvent;
        public delegate void UpdateModifiersHandler(Inventory.InventoryCell ic);
        public event UpdateModifiersHandler UpdateModfiersEvent;
        private readonly Transform additionCellsForModifiers;
        private readonly Transform activeModifiersContainer;
        public SMGEventReceiver(Transform gsData, GameObject ma, Inventory.InventoryContainer ic,
            SMGCamera cam, SMGModifiersData mD, SMGModifiersCellDescription mcd, Transform acfm, Transform acmc)
        {
            activeModifiersContainer = acmc;
            ModifiersCells = activeModifiersContainer.GetComponentsInChildren<SMGModifiersCell>().ToList();

            Sprite emptySprite = Resources.Load<Sprite>("EmptyCell");
            foreach (var c in ModifiersCells)
                c.OnInit(this, emptySprite);

            gunsCells = gsData.GetComponentsInChildren<GunsCell>().ToList();

            foreach (var c in gunsCells)
                c.OnInit(this);

            modifiersAnswer = ma;
            modifiersAnswer.SetActive(false);

            inventoryContainer = ic;
            MSMGCamera = cam;
            modifiersData = mD;
            modifiersCellDescription = mcd;
            additionCellsForModifiers = acfm;

            int additionCellsCount = 30;
            for (int i = 0; i < additionCellsCount; i++)
            {
                Object.Instantiate(ModifiersCells[0], additionCellsForModifiers).OnInit(this, emptySprite);
            }
        }
        internal void OnActivateCurrentModifierCell(SMGModifiersCell modifiersCell)
        {
            if (modifiersCell.IsEmpty)//если слот пуст
                return;

            currentGunCell.SetMag(modifiersCell.TTI.Index);

            ChangeModfierCell?.Invoke(modifiersCell);
            ChangeSelectedGunEvent?.Invoke(currentGunCell.Ic);
            MSMGCamera.SetMagToActiveGun((ModifierCharacteristics.ModifierIndex)currentGunCell.Ic.MGun.Dispenser);
            UpdateModfiersEvent?.Invoke(inventoryContainer.EventReceiver.GetSelectedCell());
        }

        internal void OnSelectGunsCell(GunsCell sMGGunsCell)
        {
            currentGunCell = sMGGunsCell;
            MSMGCamera.SetActiveGun(currentGunCell.Id);
            ChangeSelectedGunEvent?.Invoke(sMGGunsCell.Ic);
            UpdateGunCellMods();
        }
        public void UpdateGunCellMods()
        {
            MSMGCamera.SetMagToActiveGun((ModifierCharacteristics.ModifierIndex)currentGunCell.Ic.MGun.Dispenser);
            UpdateModfiersEvent?.Invoke(inventoryContainer.EventReceiver.GetSelectedCell());
        }

        /// <summary>
        /// вызов при входе в область слота модификатора
        /// </summary>
        /// <param name="cell"></param>
        public void OnEnterModifiersCell(SMGModifiersCell cell)
        {
            currentModCell = cell;
            modifiersCellDescription.ChangeModifier(currentModCell.TTI);
            ChangeSelectedGunEvent?.Invoke(currentGunCell.Ic);
            modifiersAnswer.SetActive(currentModCell);
        }

        public void SetEnable(bool v)
        {
            if (v) OnEnable();
            else OnDisable();
        }
        private void OnEnable()
        {
            ChangeSelectedGunEvent += OnChangeCurrentGunCell;
            FillGunCells();
            if (gunsCells[0].Id != 0)
            {
                currentGunCell = gunsCells[0];
                MSMGCamera.SetActiveGun(currentGunCell.Id);
                ChangeSelectedGunEvent?.Invoke(currentGunCell.Ic);
                MSMGCamera.SetMagToActiveGun((ModifierCharacteristics.ModifierIndex)currentGunCell.Ic.MGun.Dispenser);

                UpdateModfiersEvent?.Invoke(inventoryContainer.EventReceiver.GetSelectedCell());
                ChangeModfierCell?.Invoke(ModifiersCells[0]);
            }
        }
        private void OnDisable() => ChangeSelectedGunEvent -= OnChangeCurrentGunCell;

        public void OnDeselectModifiersCell()
        {
            modifiersAnswer.SetActive(false);
            currentModCell = null;
        }
        private void FillGunCells()
        {
            List<Inventory.InventoryCell> cellsContGun = inventoryContainer.GetCells().FindAll(c => Inventory.ItemStates.ItsGun(c.Id));
            //сначала очистка всех слотов
            for (int i = 0; i < gunsCells.Count; i++)
                gunsCells[i].Clear();

            //затем заполнение имеющимся оружием
            for (int i = 0; i < cellsContGun.Count; i++)
                gunsCells[i].ChangeItem(cellsContGun[i].Id, cellsContGun[i]);

            currentGunCell = gunsCells[0];
        }
        private void FillModifiersCells(ModifierCharacteristics.GunTitles title)
        {
            var modifirs = modifiersData.GetModifiersData().FindAll(m => m.Title == title);

            //сначала очистка всех слотов
            for (int i = 0; i < ModifiersCells.Count; i++)
                ModifiersCells[i].Clear();

            //затем заполнение имеющимися модами            
            for (int i = 0; i < modifirs.Count; i++)
            {
                if (ModifiersCells.Count <= i)
                {
                    var newModCell = additionCellsForModifiers.GetComponentInChildren<SMGModifiersCell>();
                    ModifiersCells.Add(newModCell);
                    newModCell.transform.SetParent(activeModifiersContainer);
                }
                ModifiersCells[i].ChangeModifier(modifirs[i]);
            }
        }
        internal void UnequipMagOnSelGun()
        {
            currentGunCell.SetMag(ModifierCharacteristics.ModifierIndex.None);
            ChangeSelectedGunEvent?.Invoke(currentGunCell.Ic);
            UpdateGunCellMods();
        }
        private void OnChangeCurrentGunCell(Inventory.InventoryCell ic) =>
            FillModifiersCells(ic ? (ModifierCharacteristics.GunTitles)ic.MGun.Title : ModifierCharacteristics.GunTitles.None);
    }
}