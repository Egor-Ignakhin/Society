using UnityEngine;

class CanvasesCreator : MonoBehaviour
{
    private void Awake()
    {
        var canvases = Resources.LoadAll<Canvas>("Canvases\\");

        foreach (var c in canvases)
        {
            Instantiate(c, transform);
        }
    }
}
