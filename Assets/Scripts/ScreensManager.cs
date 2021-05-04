/// <summary>
/// класс отвечающий за текущий экран
/// </summary>
public class ScreensManager
{
    private static IGameScreen currentScreen;
    public static void SetScreen(IGameScreen screen)
    {
        currentScreen = screen;
    }
    public static IGameScreen GetScreen() => currentScreen;
}
public interface IGameScreen
{
}
