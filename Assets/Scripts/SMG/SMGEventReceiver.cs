using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SMG
{

    public class SMGEventReceiver
    {
        private readonly List<ModifierCell> ModifiersCells = new List<ModifierCell>();//слоты с модификаторами
        private readonly List<GunCell> gunsCells = new List<GunCell>();// слоты с оружием
        private readonly GameObject modifiersAnswer;// подсказка о модификаторе        
        private ModifierCell currentModCell;//активный слот под модификации
        private GunCell currentGunCell;
        private readonly Inventory.InventoryContainer inventoryContainer;// главный обработчик инвентаря                 
        private readonly SMGModifiersData modifiersData;// контейнер под модификации
        private readonly SMGModifiersCellDescription modifiersCellDescription;// динамичное описание для модификаций

        public delegate void GunChangeHandler(Inventory.InventoryCell ic);
        public event GunChangeHandler ChangeGunEvent;

        public delegate void UpdateModifiersHandler(ModifierCell modCell);
        public event UpdateModifiersHandler UpdateModfiersEvent;

        private readonly Transform additionCellsForModifiers;
        private readonly Transform activeModifiersContainer;
        private readonly DynamicalElementsAnswer DEA;
        public SMGEventReceiver(Transform gsData, GameObject ma, Inventory.InventoryContainer ic,
            SMGModifiersData mD, SMGModifiersCellDescription mcd, Transform acfm, Transform acmc, DynamicalElementsAnswer dea)
        {
            activeModifiersContainer = acmc;
            ModifiersCells = activeModifiersContainer.GetComponentsInChildren<ModifierCell>().ToList();

            Sprite emptySprite = Resources.Load<Sprite>("EmptyCell");
            foreach (var c in ModifiersCells)
                c.OnInit(this, emptySprite);

            gunsCells = gsData.GetComponentsInChildren<GunCell>().ToList();

            foreach (var c in gunsCells)
                c.OnInit(this);

            modifiersAnswer = ma;
            modifiersAnswer.SetActive(false);

            inventoryContainer = ic;
            modifiersData = mD;
            modifiersCellDescription = mcd;
            additionCellsForModifiers = acfm;

            int additionCellsCount = 30;
            for (int i = 0; i < additionCellsCount; i++)
            {
                Object.Instantiate(ModifiersCells[0], additionCellsForModifiers).OnInit(this, emptySprite);
            }
            DEA = dea;
        }
        private ModifierCell ccCandidate;
        internal void OnSelectModifierCell(ModifierCell modifiersCell)
        {
            if (modifiersCell == currentModCell)//если слот пуст
                return;
            if (modifiersCell == ModifiersCells[0])
                return;

            ccCandidate = modifiersCell;
            DEA.Show(ReplaceModifier, null, "Установить модификатор?");
        }
        private void ReplaceModifier()
        {

            modifiersData.AddModifier(ModifierCharacteristics.SMGTitleTypeIndex.StructFromIcGun(currentGunCell.Ic.MGun));
            currentModCell = ccCandidate;

            currentGunCell.SetMag(currentModCell.TTI.Index);

            modifiersData.RemoveModifier(currentModCell.TTI);

            ReFillModifiersCells();
        }

        internal void OnSelectGunsCell(GunCell sMGGunsCell)
        {
            if (currentGunCell == sMGGunsCell)
                return;

            currentGunCell = sMGGunsCell;
            ReFillModifiersCells();
            ChangeGunEvent?.Invoke(currentGunCell.Ic);
        }

        /// <summary>
        /// вызов при входе в область слота модификатора
        /// </summary>
        /// <param name="cell"></param>
        public void OnEnterModifiersCell(ModifierCell cell)
        {
            modifiersCellDescription.ChangeModifier(cell.TTI);
            modifiersAnswer.SetActive(cell);
        }

        public void SetEnable(bool v)
        {
            if (v) OnEnable();
        }
        private void OnEnable()
        {
            ccCandidate = null;
            currentModCell = null;
            currentGunCell = null;

            ReFillGunCells();
            if (gunsCells[0].Id != 0)
                OnSelectGunsCell(gunsCells[0]);
        }

        public void OnDeselectModifiersCell() => modifiersAnswer.SetActive(false);

        private void ReFillGunCells()
        {
            List<Inventory.InventoryCell> cellsContGun = inventoryContainer.GetCells().FindAll(c => Inventory.ItemStates.ItsGun(c.Id));
            //сначала очистка всех слотов
            for (int i = 0; i < gunsCells.Count; i++)
                gunsCells[i].Clear();

            //затем заполнение имеющимся оружием
            for (int i = 0; i < cellsContGun.Count; i++)
                gunsCells[i].ChangeItem(cellsContGun[i].Id, cellsContGun[i]);
        }
        /// <summary>
        /// вызовывается для перестройки коллекции модификаторов
        /// </summary>
        /// <param name="ic"></param>
        private void ReFillModifiersCells()
        {
            ModifierCharacteristics.GunTitles title = (ModifierCharacteristics.GunTitles)currentGunCell.Ic.MGun.Title;
            var modifirs = new List<ModifierCharacteristics.SMGTitleTypeIndex>()
            {
                { ModifierCharacteristics.SMGTitleTypeIndex.StructFromIcGun(currentGunCell.Ic.MGun) }// заполнение 1 слота вставленным модификатором                        
            };
            var md = modifiersData.GetModifiersData().FindAll(m => m.Title == title);
            md.Sort((x, y) => y.Index.CompareTo(x.Index));
            modifirs.AddRange(md);

            //сначала очистка всех слотов
            for (int i = 0; i < ModifiersCells.Count; i++)
                ModifiersCells[i].Clear();
            //затем заполнение имеющимися модами            
            for (int i = 0; i < modifirs.Count; i++)
            {
                if (ModifiersCells.Count <= i)
                {
                    var newModCell = additionCellsForModifiers.GetComponentInChildren<ModifierCell>();
                    ModifiersCells.Add(newModCell);
                    newModCell.transform.SetParent(activeModifiersContainer);
                }
                ModifiersCells[i].ChangeModifier(modifirs[i], currentGunCell.Ic);
            }

            //затем уборка всех непонадобившихся слотов
            for (int i = Mathf.Clamp(modifirs.Count, 9, 100); i < ModifiersCells.Count; i++)
            {
                ModifiersCells[i].transform.SetParent(additionCellsForModifiers);
                ModifiersCells.RemoveAt(i);
            }

            UpdateModfiersEvent?.Invoke(ModifiersCells[0]);
        }
        internal void UnequipMagOnSelGun()
        {
            currentModCell = null;
            modifiersData.AddModifier(ModifierCharacteristics.SMGTitleTypeIndex.StructFromIcGun(currentGunCell.Ic.MGun));
            currentGunCell.SetMag(ModifierCharacteristics.ModifierIndex.None);
            ModifiersCells[0].Clear();
            ReFillModifiersCells();
        }
    }
}