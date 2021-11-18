using Society.GameScreens;

namespace Society.Player
{
    internal sealed class PlayerDeathHandler
    {
        public void OnDeathEvent()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesManager.DieScreenScene);
        }
    }
}