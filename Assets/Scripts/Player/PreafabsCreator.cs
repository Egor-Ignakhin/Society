using UnityEngine;

class PreafabsCreator : MonoBehaviour
{
    private void Awake()
    {
        var prefabs = Resources.LoadAll("Canvases\\");        

        foreach (var c in prefabs)
        {
            Instantiate(c, transform);
        }        
    }
}
