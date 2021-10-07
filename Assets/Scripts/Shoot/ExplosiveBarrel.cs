using Society.Enemies;
using Society.Patterns;
using Society.Player;
using Society.Player.Controllers;

using UnityEngine;
namespace Society.Shoot
{
    sealed class ExplosiveBarrel : MonoBehaviour, IBulletReceiver
    {
        private AudioSource explosionSound;
        [SerializeField] private GameObject explosion;
        [SerializeField] private GameObject barrel;
        private Transform player;
        private BasicNeeds playerNeeds;
        [Range(0, 100)] public float damageRadius;
        [Range(0, 1000)] public float maxDamage;
        [Range(0, 10000)] public float maxImpulse;


        private float damageGradient;

        private Rigidbody rb;
        private float g = 9.8f;

        public void OnBulletEnter()
        {
            g = 0;
            explosion.SetActive(true);
            barrel.SetActive(false);
            DamageEnemies();
            DamagePlayer();
            AddImpulse();
            explosionSound.Play();
        }

        private void Start()
        {
            explosion.SetActive(false);
            damageGradient = maxDamage / damageRadius;
            rb = GetComponent<Rigidbody>();
            explosionSound = GetComponent<AudioSource>();
            player = FindObjectOfType<FirstPersonController>().transform;
            playerNeeds = BasicNeeds.Instance;
        }
        private void AddImpulse()
        {
            Collider[] collidersInRadius = Physics.OverlapSphere(transform.position, damageRadius);
            foreach (Collider col in collidersInRadius)
            {
                Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(maxImpulse, transform.position, damageRadius, 3.0F, ForceMode.Impulse);
                }
            }
        }
        private void DamageEnemies()
        {
            Collider[] collidersInRadius = Physics.OverlapSphere(transform.position, damageRadius);
            foreach (Collider col in collidersInRadius)
            {
                EnemyCollision obj = col.gameObject.GetComponent<EnemyCollision>();
                if (obj != null)
                {
                    DamageEnemy(obj);
                }
            }
        }

        private void DamageEnemy(EnemyCollision enemy)
        {

            float dist = (transform.position - enemy.transform.position).magnitude;
            //Крайне упрощенная модель зависимости повреждений от дистанции
            float damage = maxDamage - damageGradient * dist;
            enemy.InjureEnemy(damage);
        }

        private void DamagePlayer()
        {
            float dist = (transform.position - player.position).magnitude;
            //Крайне упрощенная модель зависимости повреждений от дистанции
            if (dist < damageRadius)
            {
                float damage = maxDamage - damageGradient * dist;
                playerNeeds.InjurePerson(damage, 0, 1);
            }
        }

        private void FixedUpdate()
        {
            //Компенсируем гравитаию, отключенную в интересах красоты взрыва:
            rb.AddForce(0, -rb.mass * g, 0);
        }
    }
}