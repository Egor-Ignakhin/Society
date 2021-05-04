using UnityEngine;

sealed class LookAtMainCamera : MonoBehaviour
{
    private Transform mainCamT;
    private void Awake()
    {
        Transform t;
        if (FindObjectOfType<FirstPersonController>())
        {
            t = FindObjectOfType<FirstPersonController>().transform;
        }
        else
        {
            t = Camera.main.transform;
        }
        mainCamT = t;
    }
    private void Update()
    {
        transform.LookAt(mainCamT);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
}
