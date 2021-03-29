using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BarrelCampManager : MonoBehaviour
{
    private bool playerIsInside;
    public void InsidePlayer()
    {
        playerIsInside = true;
    }
    public void OutsidePlayer()
    {
        playerIsInside = false;
    }
}
