using UnityEngine;
namespace Shoots
{/// <summary>
/// патрон для оружия
/// </summary>
    public class Bullet : MonoBehaviour
    {
        private Vector3 target;//точка назначения
        private float speed;// скорость полёта
        private Enemy enemy;// возможный враг
        private bool isFinished;// долетел ли снаряд
        private GameObject impactEffect;// эффекта столкновения
        private float caliber;// калибр снаряда
        [SerializeField] private float mass = 0;
        [SerializeField] private float area = 0.649f;
        [SerializeField] private float kf = 1;
        private float distance;
        private float maxDistance;
        public void Init(BulletValues bv, RaycastHit t, GameObject impact, Enemy e)
        {
            maxDistance = bv.MaxDistance;
            distance = bv.CurrentDistance;
            this.caliber = bv.Caliber;
            target = t.point;
            speed = bv.Speed;
            impactEffect = impact;
            var parent = new GameObject("parentForImpact").transform;
            parent.SetParent(t.transform);

            impactEffect = Instantiate(impactEffect, target, Quaternion.identity, parent);

            this.enemy = e;

            impactEffect.transform.forward = t.normal;
            impactEffect.SetActive(false);
        }
        public void Init(BulletValues bv, Vector3 t)
        {
            maxDistance = bv.MaxDistance;
            target = t;
            speed = bv.Speed;
            this.caliber = bv.Caliber;
            distance = bv.CurrentDistance;
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
                    enemy.InjureEnemy(Gun.GetOptimalDamage(mass, speed, area, kf, distance, maxDistance), PlayerClasses.BasicNeeds.Instance);
                impactEffect.SetActive(true);
            }

            isFinished = true;
            Destroy(gameObject, 0.1f);
        }
    }
}