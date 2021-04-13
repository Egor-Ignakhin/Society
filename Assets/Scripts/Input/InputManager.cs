using System.Collections.Generic;
using UnityEngine;

public sealed class InputManager
{
    public static int IsLockeds { get; private set; } = 0;
    public static int IsEnableInput { get; private set; }
    public static void LockInput()
    {
        IsLockeds++;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public static void Unlock()
    {
        if (IsLockeds > 0)
            IsLockeds--;
        if (IsLockeds <= 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public static void DisableInput()
    {
        IsEnableInput++;
    }
    public static void EnableInput()
    {
        if (IsEnableInput > 0)
            IsEnableInput--;
    }
}
