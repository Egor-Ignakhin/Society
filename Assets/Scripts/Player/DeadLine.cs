namespace Society.Player
{
    public class DeadLine
    {
        private const int deadSceneIndex = 4;
        public void LoadDeadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(deadSceneIndex);
        }
    }
}