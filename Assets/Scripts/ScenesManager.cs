using PlayerClasses;
using UnityEngine;

public sealed class ScenesManager : InteractiveObject
{
    [SerializeField] private int nextScene;


    public override void Interact(PlayerStatements pl)
    {
        LoadNextScene();
    }

    public void LoadNextScene(int scene = -1)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene == -1 ? nextScene : scene);
    }
}
