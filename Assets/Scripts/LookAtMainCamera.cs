using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class LookAtMainCamera : MonoBehaviour
{
    private Transform mainCamT;
    private void Awake()
    {
        mainCamT = FindObjectOfType<FirstPersonController>().transform;
    }
    private void Update()
    {
        transform.LookAt(mainCamT);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y,0);
    }
}
