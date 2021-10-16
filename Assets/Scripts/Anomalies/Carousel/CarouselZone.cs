using UnityEngine;
namespace Society.Anomalies.Carousel
{
    internal class CarouselZone : MonoBehaviour
    {
        [SerializeField] private CarouselManager carousel;
        [SerializeField] private BoxCollider mCollider;
        private void Awake()
        {
            carousel.SetZone(mCollider);
        }
    }
}