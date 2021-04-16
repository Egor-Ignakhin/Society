using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shoots
{
    class Bullet : MonoBehaviour
    {
        private Vector3 target;
        private float speed = 75;
        private float damage;
        private PlayerClasses.BasicNeeds basicNeeds;
        private Enemy enemy;
        private bool isFinished;
        private GameObject impactEffect;
        private float caliber = 4;
        public void Init(PlayerClasses.BasicNeeds bn, float d, RaycastHit t, GameObject impact)
        {
            damage = d;
            basicNeeds = bn;
            target = t.point;
            impactEffect = impact;
            var parent = new GameObject("parentForImpact").transform;
            parent.SetParent(t.transform);

            impactEffect = Instantiate(impactEffect, target, Quaternion.identity, parent);
            if (t.transform.parent && t.transform.parent.TryGetComponent<Enemy>(out var enemy))
            {
                this.enemy = enemy;             
            }
            else
            {
                impactEffect.transform.localScale *= caliber / 10;
            }
            impactEffect.transform.forward = t.normal;
            impactEffect.SetActive(false);
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
            if (impactEffect)
            {
                if(enemy)
                    enemy.InjureEnemy(damage, basicNeeds);
                impactEffect.SetActive(true);
            }

            isFinished = true;
            Destroy(gameObject, 0.1f);
        }
    }
}