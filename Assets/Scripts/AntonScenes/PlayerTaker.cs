using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MouseAnton
{
    class PlayerTaker : MonoBehaviour
    {
        private MouseAntonManager manager;
        private Collider mCollider;
        private void Awake()
        {
            manager = MouseAntonManager.Instance;
            mCollider = GetComponent<Collider>();
        }
        private void OnTriggerEnter(Collider other)
        {
            manager.SetPlayerPosition();
        }
    }
}