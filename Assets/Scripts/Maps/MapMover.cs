using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maps
{
    class MapMover : MonoBehaviour// класс регистрирует движение и вызывает перерисовку карты
    {
        private MapDrawer mapDrawer;
        private void Awake()
        {
            mapDrawer = FindObjectOfType<MapDrawer>();
        }
        private void FixedUpdate()
        {
            if (mapDrawer)
                mapDrawer.SetCurrentRect(transform.position.x, transform.position.z, transform.localEulerAngles);
        }
    }
}