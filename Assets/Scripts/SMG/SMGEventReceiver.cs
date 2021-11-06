using Society.Inventory;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
namespace Society.SMG
{
    /// <summary>
    /// Класс-обработчик событий СМО
    /// </summary>
    public class SMGEventReceiver
    {
        private readonly List<ModifierCell> ModifiersCells = new List<ModifierCell>();//слоты с модификаторами
        private readonly List<GunCell> gunsCells = new List<GunCell>();// слоты с оружием
        private readonly GameObject modifiersAnswer;// подсказка о модификаторе        
        private ModifierCell currentModCell;//активный слот под модификации
        private GunCell currentGunCell;
        private readonly InventoryContainer inventoryContainer;// главный обработчик инвентаря                 
        private readonly SMGModifiersData modifiersData;// контейнер под модификации
        private readonly SMGModifiersCellDescription modifiersCellDescription;// динамичное описание для модификаций

        public event Action<InventoryCell> ChangeGunEvent;

        public delegate void UpdateModifiersHandler(ModifierCell modCell);
        public event UpdateModifiersHandler UpdateModfiersEvent;

        private readonly Transform additionCellsForModifiers;
        private readonly Transform activeModifiersContainer;
        private readonly DynamicalElementsAnswer DEA;
        public SMGEventReceiver(Transform gsData, GameObject ma, InventoryContainer ic,
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
                UnityEngine.Object.Instantiate(ModifiersCells[0], additionCellsForModifiers).OnInit(this, emptySprite);
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
        /// <summary>
        /// Замена модификатора на выделенном оружии
        /// </summary>
        private void ReplaceModifier()
        {
            modifiersData.AddModifier(ModifierCharacteristics.SMGTitleTypeIndex.StructFromIcGun(currentGunCell.MGun, ccCandidate.TTI.Type));

            modifiersData.RemoveModifier(ccCandidate.TTI);
            currentModCell = ccCandidate;

            currentGunCell.SetModeFromReplacedMode(currentModCell.TTI.Type, currentModCell.TTI.Index);

            ReFillModifiersCells();
        }

        /// <summary>
        /// Выделенное оружие содержит хоть один модификатор?
        /// </summary>
        /// <returns></returns>
        internal bool CurGunCellContAnyMod() =>
             currentGunCell && ((currentGunCell.MGun.Aim != 0) || (currentGunCell.MGun.Mag != 0) || (currentGunCell.MGun.Silencer != 0));

        /// <summary>
        /// При нажатии на слот с оружием
        /// </summary>
        /// <param name="Society.SMGGunsCell"></param>
        internal void OnClickGunsCell(GunCell SMGGunsCell)
        {
            if (currentGunCell == SMGGunsCell)
                return;

            currentGunCell = SMGGunsCell;
            ChangeGunEvent?.Invoke(currentGunCell.Ic);
            ReFillModifiersCells();
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

        public void OnEnable()
        {
            ccCandidate = null;
            currentModCell = null;
            currentGunCell = null;

            ReFillGunCells();
            if (!gunsCells[0].IsEmpty())
            {
                OnClickGunsCell(gunsCells[0]);
                ReFillModifiersCells();
            }
        }

        public void OnDeselectModifiersCell() => modifiersAnswer.SetActive(false);

        /// <summary>
        /// перезапись слотов с оружием
        /// </summary>
        private void ReFillGunCells()
        {
            List<InventoryCell> cellsContGun = inventoryContainer.GetCells().FindAll(c => Society.Inventory.ItemStates.ItsGun(c.Id));
            //сначала очистка всех слотов
            for (int i = 0; i < gunsCells.Count; i++)
                gunsCells[i].Clear();

            //затем заполнение имеющимся оружием
            for (int i = 0; i < cellsContGun.Count; i++)
                gunsCells[i].ChangeItem(cellsContGun[i].Id, cellsContGun[i]);
        }

        /// <summary>
        /// Снятие всех модификаций с выд. оружия
        /// </summary>
        internal void UnequipAllModsFromCurGunCell()
        {
            if (currentGunCell)
            {
                modifiersData.AddModifier(ModifierCharacteristics.SMGTitleTypeIndex.StructFromIcGun(currentGunCell.MGun, ModifierCharacteristics.ModifierTypes.Mag));
                modifiersData.AddModifier(ModifierCharacteristics.SMGTitleTypeIndex.StructFromIcGun(currentGunCell.MGun, ModifierCharacteristics.ModifierTypes.Aim));
                modifiersData.AddModifier(ModifierCharacteristics.SMGTitleTypeIndex.StructFromIcGun(currentGunCell.MGun, ModifierCharacteristics.ModifierTypes.Silencer));

                currentGunCell.SetAim(ModifierCharacteristics.ModifierIndex.None);
                currentGunCell.SetMag(ModifierCharacteristics.ModifierIndex.None);
                currentGunCell.SetSilencer(ModifierCharacteristics.ModifierIndex.None);

                ReFillModifiersCells();
            }
        }

        /// <summary>
        /// вызовывается для перестройки коллекции модификаторов
        /// </summary>
        /// <param name="ic"></param>
        public void ReFillModifiersCells()
        {
            ModifierCharacteristics.GunTitles title = (ModifierCharacteristics.GunTitles)currentGunCell.MGun.Title;
            var modifirs = new List<ModifierCharacteristics.SMGTitleTypeIndex>()
            {
                { ModifierCharacteristics.SMGTitleTypeIndex.StructFromIcGun(currentGunCell.MGun, (ModifierCharacteristics.ModifierTypes)currentGunCell.MGun.Mag) }// заполнение 1 слота вставленным модификатором                        
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
        internal void UnequipGunMod(SMGGunElement element)
        {
            currentModCell = null;

            modifiersData.AddModifier(ModifierCharacteristics.SMGTitleTypeIndex.StructFromIcGun(currentGunCell.MGun, element.GetModifierType()));
            if (element.GetModifierType() == ModifierCharacteristics.ModifierTypes.Mag)
                currentGunCell.SetMag(ModifierCharacteristics.ModifierIndex.None);
            else if (element.GetModifierType() == ModifierCharacteristics.ModifierTypes.Aim)
                currentGunCell.SetAim(ModifierCharacteristics.ModifierIndex.None);
            else if (element.GetModifierType() == ModifierCharacteristics.ModifierTypes.Silencer)
                currentGunCell.SetSilencer(ModifierCharacteristics.ModifierIndex.None);

            ModifiersCells[0].Clear();
            ReFillModifiersCells();
        }
    }
}