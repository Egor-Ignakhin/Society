using UnityEngine;
/// <summary>
/// класс отвечающий за текущий экран, помогает не путатся в открытом инвентаре, меню, смо и тд
/// </summary>
static class ScreensManager
{
    static ScreenInputReceiver screenInputReceiver;
    private static IGameScreen currentScreen;
    public static void OnInit()
    {
        screenInputReceiver = new GameObject("ScreensSystem_InputReceiver").AddComponent<ScreenInputReceiver>();
    }

    public static void SetScreen(IGameScreen screen)
    {
        currentScreen = screen;
        SetLockStateCursor();
    }
    private static void SetLockStateCursor()
    {
        Cursor.visible = HasActiveScreen();
        Cursor.lockState = HasActiveScreen() ? CursorLockMode.None : CursorLockMode.Locked;
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
    void Hide();
}
public class ScreenInputReceiver : MonoBehaviour
{
    private MenuScripts.PauseMenu.MenuPauseManager pauseManager;
    private void Start()
    {
        pauseManager = FindObjectOfType<MenuScripts.PauseMenu.MenuPauseManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnInputEcs();
        }
    }
    private void OnInputEcs()
    {
        if (ScreensManager.HasActiveScreen())
            ScreensManager.GetActiveScreen().Hide();
        else pauseManager.Enable();
    }
}
