using UnityEngine;
namespace Society.Player
{
    /// <summary>
    /// класс отвечающий за столкновения с объектами
    /// </summary>    
    internal sealed class PlayerCollisionChecked : MonoBehaviour
    {
        private readonly float minValue = 100;// минимальная инерция для счёта урона игроку
        private BasicNeeds bn;
        private Rigidbody playerRb;
        public delegate void CollisionContactHandler();
        public event CollisionContactHandler PlayerTakingDamageEvent;
        public void OnInit(BasicNeeds bn)
        {
            this.bn = bn;
            playerRb = bn.GetComponent<Rigidbody>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            float force = 0;
            float mass = playerRb.mass;
            if (collision.transform.TryGetComponent<Rigidbody>(out var rb))
            {
                mass = rb.mass;
            }

            for (int i = 0; i < collision.contacts.Length; i++)// итерация по всем точкам соприкосновения
            {
                float len = Vector3.Project(collision.relativeVelocity,
                                                collision.contacts[i].normal).magnitude;
                if (force < len) force = len;
            }

            force = mass * force * force;
            if (force > minValue * mass)// если сила больше минимальной для нанесения урона
            {
                if (bn.IsEnableDamageFromCollision)
                {
                    bn.InjurePerson(force / mass / 10);
                    PlayerTakingDamageEvent?.Invoke();
                }
            }
        }
    }
}
