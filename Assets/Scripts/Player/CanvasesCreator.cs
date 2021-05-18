using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class CanvasesCreator : MonoBehaviour
{
    private void Awake()
    {
        var canvases = Resources.LoadAll("Canvases\\");        

        foreach (var c in canvases)
        {
            Instantiate(c, transform);
        }        
    }
}
