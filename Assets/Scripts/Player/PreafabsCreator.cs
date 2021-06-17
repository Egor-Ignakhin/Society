using UnityEngine;
using UnityEngine.EventSystems;

class PreafabsCreator : MonoBehaviour
{
    private void Awake()
    {
        var parent = new GameObject("Loading_Prefabs_Data").transform;
        var prefabs = Resources.LoadAll("Canvases\\");        

        foreach (var c in prefabs)
        {
            Instantiate(c, parent);
        }
        parent.gameObject.AddComponent<EventSystem>();
        parent.gameObject.AddComponent<StandaloneInputModule>();
    }
}
