using Society.Player.Controllers;

using UnityEngine;
namespace Society.Maps
{
    internal sealed class StreetNameContainer : MonoBehaviour
    {
        private Collider playerCollider;
        private StreetNameRenderer streetNameRenderer;
        [SerializeField] private string streetName = "NONE";
        private void Start()
        {
            playerCollider = FindObjectOfType<FirstPersonController>().GetCollider();
            streetNameRenderer = FindObjectOfType<StreetNameRenderer>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other == playerCollider)
            {
                SendStreetName();
            }
        }
        private void SendStreetName()
        {
            streetNameRenderer.SetName(streetName);
        }
    }
}