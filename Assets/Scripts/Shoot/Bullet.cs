using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shoots
{/// <summary>
/// патрон для оружия
/// </summary>
    class Bullet : MonoBehaviour
    {
        private Vector3 target;//точка назначения
        private float speed;// скорость полёта
        private float damage;//урон по возможному врагу
        private Enemy enemy;// возможный враг
        private bool isFinished;// долетел ли снаряд
        private GameObject impactEffect;// эффекта столкновения
        private float caliber = 4;// калибр снаряда
        public void Init(float d, RaycastHit t, GameObject impact, float s)
        {
            damage = d;
            target = t.point;
            speed = s;
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
        public void Init(float d, Vector3 t, float s)
        {
            damage = d;
            target = t;
            speed = s;
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
        /// <summary>
        /// "взрыв" снаряда
        /// </summary>
        private void Boom()
        {
            if (impactEffect)
            {
                if (enemy)
                    enemy.InjureEnemy(damage, PlayerClasses.BasicNeeds.Instance);
                impactEffect.SetActive(true);
            }

            isFinished = true;
            Destroy(gameObject, 0.1f);
        }
    }
}