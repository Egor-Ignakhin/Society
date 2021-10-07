using UnityEngine;
using UnityEngine.EventSystems;
namespace Society.Player
{
    sealed class PrefabsCreator : MonoBehaviour
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
}