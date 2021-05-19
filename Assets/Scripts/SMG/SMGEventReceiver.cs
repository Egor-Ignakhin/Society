using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGEventReceiver
{
    private readonly List<SMGModifiersCell> ModifiersCells = new List<SMGModifiersCell>();
    private readonly List<SMGGunsCell> gunsCells = new List<SMGGunsCell>();
    private readonly GameObject modifiersAnswer;
    private SMGModifiersCell currentModCell;
    public SMGEventReceiver(Transform mcData, Transform gsData, GameObject ma)
    {
        for (int i = 0; i < mcData.childCount; i++)
            ModifiersCells.Add(mcData.GetChild(i).GetComponent<SMGModifiersCell>());

        foreach (var c in ModifiersCells)
            c.OnInit(this);

        for (int i = 0; i < gsData.childCount; i++)
            gunsCells.Add(gsData.GetChild(i).GetComponent<SMGGunsCell>());

        foreach (var c in gunsCells)
            c.OnInit(this);

        modifiersAnswer = ma;

        modifiersAnswer.SetActive(false);
    }

    internal void ActivateCurrentModifierCell(SMGModifiersCell sMGModifiersCell)
    {
        throw new NotImplementedException();
    }

    internal void SelectGunsCell(SMGGunsCell sMGGunsCell)
    {
        Debug.Log(sMGGunsCell.name);
    }

    /// <summary>
    /// вызов при нажатии на слот модификатора
    /// </summary>
    /// <param name="cell"></param>
    public void SelectModifiersCell(SMGModifiersCell cell)
    {
        currentModCell = cell;
    }
    public void Update()
    {
        modifiersAnswer.SetActive(currentModCell);
        if (modifiersAnswer.activeInHierarchy)
        {
            modifiersAnswer.transform.localPosition = Input.mousePosition;
        }
    }
    public void DeselectModifiersCell(SMGModifiersCell cell)
    {
        currentModCell = null;
    }
}
