using PlayerClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// класс отвечающий за верстак
/// </summary>
public class WorkbenchSMG : InteractiveObject, IGameScreen
{
    private SMGMain main;
    private void Start()
    {
        main = SMGMain.Instance;
    }
    public override void Interact(PlayerStatements pl)
    {
        main.SetEnableMaps(true);      
    }  
}
