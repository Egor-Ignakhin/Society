using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SMG
{

    public class SMGEventReceiver
    {
        private readonly List<SMGModifiersCell> ModifiersCells = new List<SMGModifiersCell>();
        private readonly List<SMGGunsCell> gunsCells = new List<SMGGunsCell>();
        private readonly GameObject modifiersAnswer;
        private SMGModifiersCell currentModCell;
        private readonly Inventory.InventoryContainer inventoryContainer;
        private readonly SMGGunCharsDrawer gunCharsDrawer;
        private readonly SMGCamera MSMGCamera;
        private readonly SMGModifiersData modifiersData;
        public SMGEventReceiver(Transform mcData, Transform gsData, GameObject ma, Inventory.InventoryContainer ic, SMGGunCharsDrawer gCD, SMGCamera cam, SMGModifiersData mD)
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
            gunCharsDrawer = gCD;
            MSMGCamera = cam;
            modifiersData = mD;
        }
        internal void OnActivateCurrentModifierCell(SMGModifiersCell sMGModifiersCell)
        {
            throw new NotImplementedException();
        }

        internal void OnSelectGunsCell(SMGGunsCell sMGGunsCell)
        {
            gunCharsDrawer.OnChangeSelectedGun(sMGGunsCell.Id);
            MSMGCamera.SetActiveGun(sMGGunsCell.Id);
        }

        /// <summary>
        /// вызов при нажатии на слот модификатора
        /// </summary>
        /// <param name="cell"></param>
        public void OnSelectModifiersCell(SMGModifiersCell cell)
        {
            currentModCell = cell;
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
                modifiersAnswer.transform.localPosition = Input.mousePosition;
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
                gunsCells[i].ChangeItem(0);

            //затем заполнение имеющимся оружием
            for (int i = 0; i < cellsContGun.Count; i++)
                gunsCells[i].ChangeItem(cellsContGun[i].Id);
        }
        private void FillModifiersCells()
        {
            var modifirs = modifiersData.GetModifiersData();

            //сначала очистка всех слотов
            for (int i = 0; i < ModifiersCells.Count; i++)
                ModifiersCells[i].Clear();

            //затем заполнение имеющимися модами
            Debug.Log(modifirs.Count);
            for (int i = 0; i < modifirs.Count; i++)
                ModifiersCells[i].ChangeItem(modifirs[i]);
        }
    }
}