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
                var calculateSpeed = mBv.MaxDistance / mBv.CurrentDistance;
                mBv.Speed = calculateSpeed;
                print(BulletValues.Energy(mass * kf, mBv.Speed)/ BulletValues.Energy(mass * kf, mBv.StartSpeed));
                if (enemy)
                {
                    enemy.InjureEnemy(Gun.GetOptimalDamage(mass, mBv.Speed, area, kf, mBv.CurrentDistance, mBv.MaxDistance), PlayerClasses.BasicNeeds.Instance);
                }
                else if (BulletValues.CanReflect(BulletValues.Energy(mass * kf, mBv.Speed), BulletValues.Energy(mass * kf, mBv.StartSpeed)) && mBv.Angle < 20 && Physics.Raycast(target, mBv.PossibleReflectionPoint,
                    out RaycastHit hit, mBv.MaxDistance, mBv.Layers, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.parent)
                        hit.transform.parent.TryGetComponent(out enemy);

                    mBv.Angle = Vector3.Angle(transform.position, hit.point);
                    mBv.PossibleReflectionPoint = Vector3.Reflect(transform.forward, hit.normal);
                    mBv.CurrentDistance += hit.distance;
                    target = hit.point;

                    var gg = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    gg.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    gg.GetComponent<BoxCollider>().isTrigger = true;
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