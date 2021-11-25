using System.Threading.Tasks;

using Society.Menu.Settings;

using UnityEngine;
using UnityEngine.EventSystems;
namespace Society.Player
{
    internal sealed class PrefabsCreator : MonoBehaviour
    {
        private async void Awake()
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