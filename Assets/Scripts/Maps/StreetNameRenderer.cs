using UnityEngine;
namespace Society.Maps
{
    internal sealed class StreetNameRenderer : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI textRenderer;
        internal void SetName(string streetName)
        {
            textRenderer.SetText(streetName);
        }
    }
}