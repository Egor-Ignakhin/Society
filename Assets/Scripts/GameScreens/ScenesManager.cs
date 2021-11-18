using Society.Patterns;

using UnityEngine;
namespace Society.GameScreens
{
    public sealed class ScenesManager : InteractiveObject
    {
        [SerializeField] private int nextScene;

        public override void Interact() => LoadNextScene();


        public void LoadNextScene(int scene = -1) =>
            LoadScreensManager.Instance.LoadLevel(nextScene, scene);

        public static int MainMenu = 0;
        public static int Bunker = 2;
        public static int Map = 3;
        public static int DieScreenScene = 4;
    }
}