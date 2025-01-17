﻿using UnityEngine;
namespace Society.Anomalies.Carousel
{
    public sealed class CarouselePieceII : Inventory.InventoryItem
    {
        private Rigidbody rb;
        [Range(0f, 1f)] [SerializeField] private float drag = 0.5f;
        private Vector3 dragDirection = Vector3.zero;
        private float dragMagnitude = 0;
        private bool instantiateByAnomaly = false;
        [SerializeField] private GameObject particleEffects;

        protected override void Awake()
        {
            rb = GetComponent<Rigidbody>();

            base.Awake();
        }

        public void AddImpulse(Vector3 imp)
        {
            rb.AddForce(imp, ForceMode.Impulse);
        }


        private void FixedUpdate()
        {
            if (!instantiateByAnomaly)
                return;

            dragDirection = -rb.velocity.normalized;
            dragMagnitude = rb.velocity.sqrMagnitude * drag;
            rb.AddForce(dragMagnitude * dragDirection);
        }
        public void EnableParticleEffect()
        {
            instantiateByAnomaly = true;
            particleEffects.SetActive(true);
        }
    }
}