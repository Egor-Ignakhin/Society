using PlayerClasses;
using UnityEngine;

public sealed class ScenesManager : InteractiveObject
{
    [SerializeField] private int nextScene;
    public override void Interact(PlayerStatements pl)
    {
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
    }
}
