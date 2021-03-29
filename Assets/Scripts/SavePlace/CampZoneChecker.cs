using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CampZoneChecker : MonoBehaviour
{
    [SerializeField] private BarrelCampManager campManager;
    private CapsuleCollider playerCollider;
    private void Start()
    {
        playerCollider = FirstPersonController.GetCollider();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
        {
            campManager.InsidePlayer();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == playerCollider)
        {
            campManager.OutsidePlayer();
        }
    }
}
