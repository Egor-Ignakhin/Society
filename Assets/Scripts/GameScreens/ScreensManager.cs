using Society.Menu.MenuPause;

using UnityEngine;
namespace Society.GameScreens
{
    /// <summary>
    /// класс отвечающий за текущий экран, помогает не путатся в открытом инвентаре, меню, смо и тд
    /// </summary>
    internal static class ScreensManager
    {
        private static IGameScreen activeScreen;
        public static void OnInit()
        {
            new GameObject("ScreensSystem_InputReceiver").AddComponent<ScreenInputReceiver>();
        }

        public static void SetScreen(IGameScreen screen, bool slsc = true)
        {
            activeScreen = screen;
            if (slsc)
                SetLockStateCursor();
        }
        public static void ClearScreen()
        {
            activeScreen = null;
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
        public static bool HasActiveScreen() => activeScreen != null;
        /// <summary>
        /// возвращает активный экран
        /// </summary>
        /// <returns></returns>
        public static IGameScreen GetActiveScreen() => activeScreen;
    }
    public interface IGameScreen
    {
        bool Hide();
        KeyCode HideKey();
    }
    public class ScreenInputReceiver : MonoBehaviour
    {
        private MenuPauseManager pauseManager;
        private void Start()
        {
            pauseManager = FindObjectOfType<MenuPauseManager>();
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
            else 
                pauseManager.EnableMenu();
        }
    }
}