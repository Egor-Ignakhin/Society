using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shoots
{
    class Bullet : MonoBehaviour
    {
        private Vector3 target;
        private float speed = 50;
        private float damage;
        private PlayerClasses.BasicNeeds basicNeeds;
        [SerializeField] private LayerMask layerMask;
        public void Init(PlayerClasses.BasicNeeds bn, float d, Vector3 t)
        {
            damage = d;
            basicNeeds = bn;
            target = t;
        }
        private void Update()
        {
            Ray ray = new Ray(transform.position, target);

            if (Physics.Raycast(ray, out RaycastHit hit, 0.025f, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform != null)
                {
                    if (hit.transform.TryGetComponent<Enemy>(out var enemy))
                    {
                        enemy.InjureEnemy(damage, basicNeeds);
                    }
                    Boom();
                }
            }
            transform.Translate(target * Time.deltaTime * speed, Space.World);
        }
        private void Boom()
        {
            Destroy(gameObject, 1);
        }
    }
}