using UnityEngine;
namespace Shoots
{/// <summary>
/// патрон для оружия
/// </summary>
    public class Bullet : MonoBehaviour
    {
        private Vector3 target;//точка назначения
        private Enemy enemy;// возможный враг
        private bool isFinished;// долетел ли снаряд
        private GameObject impactEffect;// эффекта столкновения

        [SerializeField] private float mass = 0;
        [SerializeField] private float area = 0.649f;
        [SerializeField] private float kf = 1;

        BulletValues mBv;
        public void Init(BulletValues bv, RaycastHit t, GameObject impact, Enemy e)
        {
            mBv = bv;
            target = t.point;

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
            mBv = bv;
            target = t;
        }

        private void Update()
        {
            if (this.isFinished)
                return;

            if (transform.position == target)//isFinished
                Boom();
            else
                transform.position = Vector3.MoveTowards(transform.position, target, mBv.Speed * Time.deltaTime);

            //mBv.Speed -= Time.deltaTime * mBv.StartSpeed;
        }
        /// <summary>
        /// "взрыв" снаряда
        /// </summary>
        private void Boom()
        {
            if (impactEffect)
            {
                mBv.Speed = mBv.MaxDistance / mBv.CurrentDistance;
                print(mBv.Angle);
                if (enemy)
                {
                    enemy.InjureEnemy(Gun.GetOptimalDamage(mass, mBv.Speed, area, kf, mBv.CurrentDistance, mBv.MaxDistance), PlayerClasses.BasicNeeds.Instance);
                }
                else if (BulletValues.CanReflect(BulletValues.Energy(mass * kf, mBv.Speed), BulletValues.Energy(mass * kf, mBv.StartSpeed), mBv.Speed, mBv.Angle) && Physics.Raycast(target, mBv.PossibleReflectionPoint,
                    out RaycastHit hit, mBv.MaxDistance, mBv.Layers, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.parent)
                        hit.transform.parent.TryGetComponent(out enemy);

                    mBv.SetValues(hit.distance + mBv.CurrentDistance, Vector3.Reflect(transform.forward, hit.normal), Mathf.Abs(90 - Vector3.Angle(transform.forward, hit.normal)));
                    target = hit.point;

                    var gg = new GameObject("Source");
                    gg.AddComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Guns\\BulletReflect"));
                    gg.transform.position = hit.point;
                    return;
                }
                impactEffect.SetActive(true);
            }

            isFinished = true;

            Destroy(gameObject, 0.1f);
        }
    }
}