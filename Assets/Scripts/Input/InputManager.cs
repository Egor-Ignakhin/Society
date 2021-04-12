using System.Collections.Generic;

public sealed class InputManager
{
    public static int IsLockeds { get; private set; }  = 0;
    public static void LockInput()
    {
        IsLockeds++;
    }
    public static void Unlock()
    {
        IsLockeds--;
    }
}
