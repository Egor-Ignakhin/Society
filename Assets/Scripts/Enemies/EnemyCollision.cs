using Society.Patterns;
using Society.Shoot;

using UnityEngine;
namespace Society.Enemies
{
    public sealed class EnemyCollision : MonoBehaviour, IBulletReceiver
    {
        private bool wasKilled;
        [SerializeField] private Enemy mParent;
        private void Awake()
        {
            mParent.DeathEvent += Death;
        }
        private void Death()
        {
            wasKilled = true;
        }
        public void InjureEnemy(float value, bool isPlayerDamage = true)
        {
            if (!wasKilled)
                mParent.InjureEnemy(value, isPlayerDamage);
        }
        private void OnDisable()
        {
            mParent.DeathEvent -= Death;
        }
        internal void SetEnemyParent(Enemy eParent)
        {
            mParent = eParent;
        }
        public void DebuffEnemy(EnemyDebuffs d)
        {
            mParent.DebuffEnemy(d);
        }

        public void OnBulletEnter(BulletType inputBulletType)
        {
            
        }
    }
}