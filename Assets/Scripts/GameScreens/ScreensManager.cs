using UnityEngine;
/// <summary>
/// класс отвечающий за текущий экран, помогает не путатся в открытом инвентаре, меню, смо и тд
/// </summary>
static class ScreensManager
{
    private static IGameScreen currentScreen;
    public static void OnInit()
    {
        new GameObject("ScreensSystem_InputReceiver").AddComponent<ScreenInputReceiver>();
    }

    public static void SetScreen(IGameScreen screen, bool slsc = true)
    {
        currentScreen = screen;
        if (slsc)
            SetLockStateCursor();
    }
    public static void ClearScreen()
    {
        currentScreen = null;
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
    bool Hide();
    KeyCode HideKey();
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
        KeyCode bb = (ScreensManager.GetActiveScreen() != null) ? ScreensManager.GetActiveScreen().HideKey() : KeyCode.Escape;
        if (Input.GetKeyDown(bb))
        {
            OnInputBackButton();
        }
    }
    private void OnInputBackButton()
    {
        if (ScreensManager.HasActiveScreen())
        {
            if (ScreensManager.GetActiveScreen().Hide())
                ScreensManager.ClearScreen();
        }
        else pauseManager.Enable();
    }
}
