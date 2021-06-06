using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarouselAnomaly
{
    class CarouselZone : MonoBehaviour
    {
        [SerializeField] private CarouselManager carousel;
        [SerializeField] private BoxCollider mCollider;
        private void Awake()
        {
            carousel.SetZone(mCollider);
        }
    }
}