using UnityEngine;

class CanvasesCreator : MonoBehaviour
{
    private void Awake()
    {
        Instantiate(Resources.Load<Canvas>("Canvases\\[Canvas] DialogsCanvas"), transform);
        Instantiate(Resources.Load<Canvas>("Canvases\\[Canvas]MapOfWorld"), transform);
        Instantiate(Resources.Load<Canvas>("Canvases\\[Canvas] EffectsCanvas"), transform);
        Instantiate(Resources.Load<Canvas>("Canvases\\[Canvas] InventoryCanvas"), transform);
        Instantiate(Resources.Load<Canvas>("Canvases\\[Canvas] TaskCanvas"), transform);
        Instantiate(Resources.Load<Canvas>("Canvases\\[Canvas]BasicNeeds"), transform);      
    }

}
