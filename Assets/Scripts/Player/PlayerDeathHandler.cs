using Society.GameScreens;

namespace Society.Player
{
    internal sealed class PlayerDeathHandler
    {
        public void OnDeathEvent()
        {
            LoadScreensManager.Instance.LoadScene((int)Scenes.DieScreenScene);
        }
    }
}