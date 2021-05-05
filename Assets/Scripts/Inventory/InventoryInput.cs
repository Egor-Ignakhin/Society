using UnityEngine;

/// <summary>
/// класс отвечающий за считывание ввода пользователя и передачи её управляющему классу
/// </summary>
sealed class InventoryInput : Singleton<InventoryInput>
{
    public delegate void EventHandler(bool value);
    public static event EventHandler ChangeActiveEvent;

    public delegate void InputHandler(int s);
    public static event InputHandler InputKeyEvent;

    private const KeyCode changeActiveKeyCode = KeyCode.E;
    private bool isEnabled;
    private FirstPersonController fps;
    private void Awake()
    {
        fps = FindObjectOfType<FirstPersonController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(changeActiveKeyCode) && InputManager.IsEnableInput == 0)
        {
            ChangeActive(isEnabled = !isEnabled);
        }
        if (Input.anyKeyDown)
        {
            SelectCell(Input.inputString);
        }
    }
    /// <summary>
    /// включение видимости инвентаря
    /// </summary>
    private void ChangeActive(bool value) { ChangeActiveEvent?.Invoke(value); }
    /// <summary>
    /// насильное выключение инвентаря
    /// </summary>
    public void DisableInventory()
    {
        ChangeActive(false);
        isEnabled = false;
    }
    public void EnableInventory()
    {
        ChangeActive(true);
        isEnabled = true;
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

    internal void DropItem(InventoryItem inventoryItem, int count)
    {
        var item = Instantiate(inventoryItem, fps.transform.position, fps.transform.rotation);
        item.SetCount(count);
    }
}
