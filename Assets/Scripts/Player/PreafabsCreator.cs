using UnityEngine;
using UnityEngine.EventSystems;

class PreafabsCreator : MonoBehaviour
{
    private void Awake()
    {
        var parent = GameObject.Find("UI").transform;
        var prefabs = Resources.LoadAll("Canvases\\");        

        foreach (var c in prefabs)
        {
            Instantiate(c, parent);
        }
        parent.gameObject.AddComponent<EventSystem>();
        parent.gameObject.AddComponent<StandaloneInputModule>();
    }
}
