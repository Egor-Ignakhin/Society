using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += new Vector3(0, 30, 0) * Time.deltaTime;
    }
}
