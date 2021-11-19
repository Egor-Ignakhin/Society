using Society.Menu.MenuPause;

internal class GameSettings
{

    public static float MinFov => MenuPauseManager.GetCurrentGameSettings().minFov;

    public static float MaxFov => MenuPauseManager.GetCurrentGameSettings().maxFov;

    public static float Fov => MenuPauseManager.GetCurrentGameSettings().FOV;

    public static float Sensivity => MenuPauseManager.GetCurrentGameSettings().Sensivity;
}