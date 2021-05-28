using UnityEngine;
namespace Maps
{
    public class StreetNameRenderer : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI textRenderer;
        internal void SetName(string streetName)
        {
            textRenderer.SetText(streetName);
        }
    }
}