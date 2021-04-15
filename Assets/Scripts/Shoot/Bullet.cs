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
        private Enemy enemy;
        private bool isFinished;
        public void Init(PlayerClasses.BasicNeeds bn, float d, RaycastHit t)
        {
            damage = d;
            basicNeeds = bn;
            target = t.point;
            if (t.transform.TryGetComponent<Enemy>(out var enemy))
                this.enemy = enemy;
        }
        public void Init(PlayerClasses.BasicNeeds bn, float d, Vector3 t)
        {
            damage = d;
            basicNeeds = bn;
            target = t;
        }

        private void Update()
        {
            if (this.isFinished)
                return;
         
            if (transform.position == target)//isFinished
                Boom();
            else
                transform.position = Vector3.MoveTowards(transform.position, target, 0.01f * speed);
        }

        private void Boom()
        {
            if (enemy)
            {
                enemy.InjureEnemy(damage, basicNeeds);
            }

            isFinished = true;
            Destroy(gameObject, 0.1f);
        }
    }
}