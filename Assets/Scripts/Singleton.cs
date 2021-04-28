using UnityEngine;
using UnityEngine.SceneManagement;
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected bool isInitialized = false;
    private static bool wasDestroyed = false;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (wasDestroyed)
                return null;

            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject(typeof(T).ToString());

                    instance = singleton.AddComponent<T>();
                }
            }
            var heir = instance as Singleton<T>;
            if (!heir.isInitialized)
                heir.Init();
            return instance;
        }

    }
    public virtual void Init()
    {
        isInitialized = true;
        SceneManager.sceneLoaded += OnLevelLoad;
    }
    private void OnLevelLoad(Scene scene, LoadSceneMode mode)
    {
        wasDestroyed = false;
    }
    protected void OnDestroy()
    {
        if (!isInitialized)
            Init();
        wasDestroyed = true;
    }
}

