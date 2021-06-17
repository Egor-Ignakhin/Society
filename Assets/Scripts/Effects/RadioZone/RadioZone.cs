using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioZone : MonoBehaviour
{
    private Collider playerCol;
    [SerializeField] private AudioSource radio;
    private void Start()
    {
        playerCol = FindObjectOfType<FirstPersonController>().GetCollider();
        radio.volume = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other == playerCol)
        {
            radio.volume = 1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == playerCol)
        {
            radio.volume = 0;
        }
    }
}
