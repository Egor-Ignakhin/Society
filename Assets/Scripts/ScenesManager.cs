using PlayerClasses;
using UnityEngine;

public sealed class ScenesManager : InteractiveObject
{
    [SerializeField] private int nextScene;

    public override void Interact(PlayerStatements pl) => LoadNextScene();


    public void LoadNextScene(int scene = -1)
    {
        LoadScreensManager.Instance.LoadLevel(nextScene, scene);
    }
    public static int MainMenu = 0;
}
