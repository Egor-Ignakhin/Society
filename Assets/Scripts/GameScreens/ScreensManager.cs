/// <summary>
/// класс отвечающий за текущий экран, помогает не путатся в открытом инвентаре, меню, смо и тд
/// </summary>
static class ScreensManager
{
    private static IGameScreen currentScreen;
    public static void SetScreen(IGameScreen screen)
    {
        currentScreen = screen;
        SetLockStateCursor();
    }
    private static void SetLockStateCursor()
    {
        UnityEngine.Cursor.visible = HasActiveScreen();
        UnityEngine.Cursor.lockState = HasActiveScreen() ? UnityEngine.CursorLockMode.None : UnityEngine.CursorLockMode.Locked;
    }

    /// <summary>
    /// возвращает true если активен какой-либо экран
    /// </summary>
    /// <returns></returns>
    public static bool HasActiveScreen() => currentScreen != null;
    /// <summary>
    /// возвращает активный экран
    /// </summary>
    /// <returns></returns>
    public static IGameScreen GetActiveScreen() => currentScreen;
}
public interface IGameScreen
{
}
