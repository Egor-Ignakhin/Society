using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogLairChecker : MonoBehaviour
{
    [SerializeField] private BloodDogsLair lair;
    private CapsuleCollider playerCollider;
    private void Start()
    {
        playerCollider = FirstPersonController.GetCollider();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == playerCollider)
        {
            lair.InsidePlayer();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == playerCollider)
        {
            lair.OutsidePlayer();
        }
    }
}
