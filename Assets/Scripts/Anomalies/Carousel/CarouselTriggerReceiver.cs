using UnityEngine;

namespace Society.Anomalies.Carousel
{
    internal class CarouselTriggerReceiver : MonoBehaviour
    {
        [SerializeField] private CarouselAnomalyManager manager;
        private void Start() => manager.InitCollider(GetComponent<Collider>());

        private void OnTriggerEnter(Collider other) => manager.AddInList(other.attachedRigidbody);

        private void OnTriggerExit(Collider other) => manager.RemoveFromList(other.attachedRigidbody);
    }
}