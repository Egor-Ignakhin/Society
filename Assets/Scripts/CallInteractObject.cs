using PlayerClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CallInteractObject : InteractiveObject
{
    [SerializeField]private InteractiveObject receiver;

    public override void Interact(PlayerStatements pl)
    {
        receiver.Interact(pl);        
    }
}
