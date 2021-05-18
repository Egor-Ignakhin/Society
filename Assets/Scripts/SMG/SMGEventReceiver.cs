using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGEventReceiver
{
    private readonly List<SMGModifiersCell> ModifiersCells = new List<SMGModifiersCell>();
    private readonly List<SMGGunsCell> gunsCells = new List<SMGGunsCell>();
    public SMGEventReceiver(Transform mcData, Transform gsData)
    {
        for (int i = 0; i < mcData.childCount; i++)
            ModifiersCells.Add(mcData.GetChild(i).GetComponent<SMGModifiersCell>());

        foreach (var c in ModifiersCells)
            c.OnInit(this);

        for (int i = 0; i < gsData.childCount; i++)
            gunsCells.Add(gsData.GetChild(i).GetComponent<SMGGunsCell>());

        foreach (var c in gunsCells)
            c.OnInit(this);
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
        Debug.Log(cell.name);
    }
}
