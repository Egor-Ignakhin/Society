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
        private readonly RectTransform modifiersAnswerTrans;// подсказка о модификаторе
        private SMGModifiersCell currentModCell;//активный слот под модификации
        private SMGGunsCell currentGunCell;
        private readonly Inventory.InventoryContainer inventoryContainer;// главный обработчик инвентаря
        private readonly SMGGunCharsDrawer gunCharsDrawer;// отрисовщик выделенного оружия

        internal void UnequipMagOnSelGun()
        {
            currentGunCell.SetMag(SMGModifierCharacteristics.ModifierIndex.None);
        }

        private readonly SMGCamera MSMGCamera;// камера для отрисовки окна предпросмотра
        private readonly SMGModifiersData modifiersData;// контейнер под модификации
        private readonly SMGModifiersCellDescription modifiersCellDescription;// динамичное описание для модификаций
        public delegate void ChangeCellHandler(SMGModifiersCell t);
        public event ChangeCellHandler ChangeModfierCell;// событие смены активного модификатора
        public SMGEventReceiver(Transform mcData, Transform gsData, GameObject ma, Inventory.InventoryContainer ic,
            SMGGunCharsDrawer gCD, SMGCamera cam, SMGModifiersData mD, SMGModifiersCellDescription mcd)
        {
            ModifiersCells = mcData.GetComponentsInChildren<SMGModifiersCell>().ToList();

            foreach (var c in ModifiersCells)
                c.OnInit(this);

            gunsCells = gsData.GetComponentsInChildren<SMGGunsCell>().ToList();

            foreach (var c in gunsCells)
                c.OnInit(this);

            modifiersAnswer = ma;
            modifiersAnswerTrans = modifiersAnswer.GetComponent<RectTransform>();
            modifiersAnswer.SetActive(false);

            inventoryContainer = ic;
            gunCharsDrawer = gCD;
            MSMGCamera = cam;
            modifiersData = mD;
            modifiersCellDescription = mcd;
        }
        internal void OnActivateCurrentModifierCell(SMGModifiersCell modifiersCell)
        {
            if (modifiersCell.IsEmpty)//если слот пуст
                return;
        
                currentGunCell.SetMag(modifiersCell.mTTI.Index);

            ChangeModfierCell?.Invoke(modifiersCell);
        }

        internal void OnSelectGunsCell(SMGGunsCell sMGGunsCell)
        {
            currentGunCell = sMGGunsCell;
            gunCharsDrawer.OnChangeSelectedGun(currentGunCell.Id);
            MSMGCamera.SetActiveGun(currentGunCell.Id);
        }

        /// <summary>
        /// вызов при нажатии на слот модификатора
        /// </summary>
        /// <param name="cell"></param>
        public void OnSelectModifiersCell(SMGModifiersCell cell)
        {
            currentModCell = cell;
            modifiersCellDescription.ChangeModifier(currentModCell.mTTI);
        }

        internal void OnEnable()
        {
            FillGunCells();
            FillModifiersCells();
            if (gunsCells[0].Id != 0)
            {
                gunCharsDrawer.OnChangeSelectedGun(gunsCells[0].Id);
                MSMGCamera.SetActiveGun(gunsCells[0].Id);                
            }
        }

        public void OnUpdate()
        {
            modifiersAnswer.SetActive(currentModCell);
            if (modifiersAnswer.activeInHierarchy)
            {
                modifiersAnswerTrans.localPosition = Input.mousePosition;
            }
        }
        public void OnDeselectModifiersCell()
        {
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
        private void FillModifiersCells()
        {
            var modifirs = modifiersData.GetModifiersData();

            //сначала очистка всех слотов
            for (int i = 0; i < ModifiersCells.Count; i++)
                ModifiersCells[i].Clear();

            //затем заполнение имеющимися модами            
            for (int i = 0; i < modifirs.Count; i++)
                ModifiersCells[i].ChangeItem(modifirs[i]);
        }
    }
}