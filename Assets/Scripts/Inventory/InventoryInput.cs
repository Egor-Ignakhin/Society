using UnityEngine;

sealed class InventoryInput : Singleton<InventoryInput>
{
    public delegate void EventHandler(bool value);
    public static event EventHandler ChangeActiveEvent;

    public delegate void InputHandler(int s);
    public static event InputHandler InputKeyEvent;

    private const KeyCode changeActiveKeyCode = KeyCode.E;
    private void Update()
    {
        if (Input.GetKeyDown(changeActiveKeyCode) && InputManager.IsEnableInput == 0)
        {
            ChangeActive(true);
        }
        if (Input.anyKeyDown)
        {
            SelectCell(Input.inputString);
        }
    }
    /// <summary>
    /// включение видимости инвентаря
    /// </summary>
    private void ChangeActive(bool value) => ChangeActiveEvent?.Invoke(value);
    public void SimularActive(bool value)
    {
        ChangeActive(value);
    }

    /// <summary>
    /// выделение слотов при нажатии клавиш
    /// </summary>
    /// <param name="input"></param>
    private void SelectCell(string input)
    {
        if (input == string.Empty)
            return;

        if (int.TryParse(input, out int s))
            InputKeyEvent?.Invoke(s);
    }
}
