using UnityEngine;

namespace Enemies
{
    public sealed class BredEnemy : Enemy
    {
        private void OnEnable()
        {
            Init(2, 10, seeDistance, health);
        }

        protected override void LookOnTarget()
        {
            Vector3 startRot = transform.localEulerAngles;
            transform.LookAt(enemy ? enemy.transform : target);
            transform.localEulerAngles = new Vector3(startRot.x, transform.localEulerAngles.y, 0);            
        }        

        protected override string Type()
        {
            return TypesEnemies.Bred;
        }
    }
}