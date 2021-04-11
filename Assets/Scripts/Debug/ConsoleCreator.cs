using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class ConsoleCreator : MonoBehaviour
{
    private bool isShowed;
    private GameObject mainParent;
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        mainParent = Instantiate(Resources.Load<GameObject>("Debug\\[Canvas] DebugConsole"));
        mainParent.transform.SetParent(transform);
    }
    private void SetEnable()
    {
        isShowed = !isShowed;

    }
    private void PlayAnim()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetEnable();
        }
        if (isShowed)
        {
            PlayAnim();
        }
    }
}
