using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntonController : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FindObjectOfType<FirstPersonController>().CanSprint = false;
        FindObjectOfType<FirstPersonController>().WalkSpeedInternal = 1;
    }
}
