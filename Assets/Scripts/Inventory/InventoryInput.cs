using UnityEngine;

public sealed class InventoryInput : MonoBehaviour
{
    public delegate void EventHandler();
    public static event EventHandler ChangeActiveEvent;

    private const KeyCode changeActiveKeyCode = KeyCode.E;
    private void Update()
    {
        if (Input.GetKeyDown(changeActiveKeyCode) && InputManager.IsEnableInput == 0)
        {
            ChangeActive();
        }
    }
    /// <summary>
    /// включение видимости инвентаря
    /// </summary>
    private void ChangeActive()
    {
        ChangeActiveEvent?.Invoke();
    }
}
