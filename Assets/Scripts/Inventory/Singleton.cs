using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    GameObject singleton = new GameObject(typeof(T).ToString());
                    instance = singleton.AddComponent<T>();
                }
            }
            return instance;
        }        
    }
}
