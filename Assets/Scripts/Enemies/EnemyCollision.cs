using System.Collections.Generic;

using Society.Patterns;
using Society.Shoot;

using UnityEngine;
namespace Society.Enemies
{
    public sealed class EnemyCollision : MonoBehaviour, IBulletReceiver
    {
        private bool wasKilled;
        [SerializeField] private Enemy mParent;
        [SerializeField] private EnemyCollisionType enemyCollisionType;

        private void Awake() => mParent.DeathEvent += Death;
        private void Death() => wasKilled = true;
        public void InjureEnemy(float value, bool isPlayerDamage = true)
        {
            if (wasKilled)
                return;

            //print(enemyCollisionType.color);
            //print(value);
                value *= enemyCollisionType.GetBlockCoefficient();
          //  print(value);
            mParent.InjureEnemy(value, isPlayerDamage);
        }
        private void OnDisable() => mParent.DeathEvent -= Death;
        internal void SetEnemyParent(Enemy eParent) => mParent = eParent;
        public void DebuffEnemy(EnemyDebuffs d) => mParent.DebuffEnemy(d);

        public void OnBulletEnter(BulletType inputBulletType)
        {

        }

        public Transform GetCenter() => mParent.GetCenter();

        [System.Serializable]
        public sealed class EnemyCollisionType
        {
            public enum EnemyType
            {
                Human,
                Bred,
                BloodDog
            }
            public enum ECColor
            {
                Green,
                Yellow,
                Red
            }
            private readonly Dictionary<(EnemyType enemyType, ECColor color), (float lowBorder, float highBorder)> blockCoedffient =
                new Dictionary<(EnemyType, ECColor), (float lowBorder, float highBorder)>
                {
                    { (EnemyType.Bred, ECColor.Green), (1.0F, 1.2F)},
                    { (EnemyType.Bred, ECColor.Yellow), (1.2F, 1.5F)},
                    { (EnemyType.Bred, ECColor.Red), (1.5F, 1.7F)}
                };

            public EnemyType type;
            public ECColor color;

            internal float GetBlockCoefficient()
            {
                var (lowBorder, highBorder) = blockCoedffient[(type, color)];
                return Random.Range(lowBorder, highBorder);
            }
        }
    }
}