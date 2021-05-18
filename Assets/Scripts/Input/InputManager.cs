using UnityEngine;
/// <summary>
/// класс блокирующий управление
/// </summary>
public sealed class InputManager
{
    public static int IsLockeds { get; private set; } = 0;
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
}
