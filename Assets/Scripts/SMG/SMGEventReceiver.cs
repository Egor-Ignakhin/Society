using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SMG
{

    public class SMGEventReceiver
    {
        private readonly List<SMGModifiersCell> ModifiersCells = new List<SMGModifiersCell>();//слоты с модификаторами
        private readonly List<SMGGunsCell> gunsCells = new List<SMGGunsCell>();// слоты с оружием
        private readonly GameObject modifiersAnswer;// подсказка о модификаторе        
        private SMGModifiersCell currentModCell;//активный слот под модификации
        private SMGGunsCell currentGunCell;
        private readonly Inventory.InventoryContainer inventoryContainer;// главный обработчик инвентаря    

        internal void UnequipMagOnSelGun()
        {
            currentGunCell.SetMag(ModifierCharacteristics.ModifierIndex.None);
            ChangeSelectedGunEvent?.Invoke(currentGunCell.Ic);
        }
        private void OnChangeCurrentGunCell(Inventory.InventoryCell ic)
        {
            FillModifiersCells((ModifierCharacteristics.GunTitles)ic.mSMGGun.Title);
        }

        private readonly SMGCamera MSMGCamera;// камера для отрисовки окна предпросмотра
        private readonly SMGModifiersData modifiersData;// контейнер под модификации
        private readonly SMGModifiersCellDescription modifiersCellDescription;// динамичное описание для модификаций
        public delegate void ChangeCellHandler(SMGModifiersCell t);
        public event ChangeCellHandler ChangeModfierCell;// событие смены активного модификатора
        public delegate void ChangeGunModsHandler(Inventory.InventoryCell ic);
        public event ChangeGunModsHandler ChangeSelectedGunEvent;
        public SMGEventReceiver(Transform mcData, Transform gsData, GameObject ma, Inventory.InventoryContainer ic,
            SMGCamera cam, SMGModifiersData mD, SMGModifiersCellDescription mcd)
        {
            ModifiersCells = mcData.GetComponentsInChildren<SMGModifiersCell>().ToList();

            foreach (var c in ModifiersCells)
                c.OnInit(this);

            gunsCells = gsData.GetComponentsInChildren<SMGGunsCell>().ToList();

            foreach (var c in gunsCells)
                c.OnInit(this);

            modifiersAnswer = ma;
            modifiersAnswer.SetActive(false);

            inventoryContainer = ic;
            MSMGCamera = cam;
            modifiersData = mD;
            modifiersCellDescription = mcd;
            ChangeSelectedGunEvent += OnChangeCurrentGunCell;
        }
        internal void OnActivateCurrentModifierCell(SMGModifiersCell modifiersCell)
        {
            if (modifiersCell.IsEmpty)//если слот пуст
                return;

            currentGunCell.SetMag(modifiersCell.mTTI.Index);

            ChangeModfierCell?.Invoke(modifiersCell);
            ChangeSelectedGunEvent?.Invoke(currentGunCell.Ic);
            MSMGCamera.GetActiveGun().GetComponentInChildren<GunModifiersActiveManager>().SetMag(ModifierCharacteristics.ModifierIndex._1);
        }

        internal void OnSelectGunsCell(SMGGunsCell sMGGunsCell)
        {
            currentGunCell = sMGGunsCell;
            MSMGCamera.SetActiveGun(currentGunCell.Id);
            ChangeSelectedGunEvent?.Invoke(sMGGunsCell.Ic);
            MSMGCamera.GetActiveGun().GetComponentInChildren<GunModifiersActiveManager>().UpdateModifiers();
        }

        /// <summary>
        /// вызов при нажатии на слот модификатора
        /// </summary>
        /// <param name="cell"></param>
        public void OnSelectModifiersCell(SMGModifiersCell cell)
        {
            currentModCell = cell;
            modifiersCellDescription.ChangeModifier(currentModCell.mTTI);
            ChangeSelectedGunEvent?.Invoke(currentGunCell.Ic);
            modifiersAnswer.SetActive(currentModCell);
        }

        internal void OnEnable()
        {
            FillGunCells();            
            if (gunsCells[0].Id != 0)
            {
                currentGunCell = gunsCells[0];
                MSMGCamera.SetActiveGun(currentGunCell.Id);
                ChangeSelectedGunEvent?.Invoke(currentGunCell.Ic);
                MSMGCamera.GetActiveGun().GetComponentInChildren<GunModifiersActiveManager>().UpdateModifiers();
            }            
        }
        public void OnDisable()
        {
            ChangeSelectedGunEvent -= OnChangeCurrentGunCell;
        }
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
                gunsCells[i].ChangeItem(0, null);

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
                ModifiersCells[i].ChangeItem(modifirs[i]);
            }
        }
    }
}