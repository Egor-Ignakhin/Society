using System.Text.RegularExpressions;
using UnityEngine;

public sealed class InventoryInput : MonoBehaviour
{
    public delegate void EventHandler();
    public static event EventHandler ChangeActiveEvent;

    public delegate void InputHandler(char s);
    public static event InputHandler InputKeyEvent;

    private const KeyCode changeActiveKeyCode = KeyCode.E;
    private void Update()
    {
        if (Input.GetKeyDown(changeActiveKeyCode) && InputManager.IsEnableInput == 0)
        {
            ChangeActive();
        }
        if (Input.anyKeyDown)
        {
            SelectCell(Input.inputString);
        }
    }
    /// <summary>
    /// включение видимости инвентаря
    /// </summary>
    private void ChangeActive()
    {
        ChangeActiveEvent?.Invoke();
    }
    private void SelectCell(string input)
    {           
        try
        {
            char s = char.Parse(input);
            if(char.IsDigit(s))
                InputKeyEvent?.Invoke(s);
        }
        catch { }
    }
}
