using UnityEngine;

namespace Enemies
{
    public sealed class BredEnemy : Enemy
    {
        private void OnEnable()
        {
            base.Init(2, 10, 25, 250);
        }

        protected override void LookOnTarget()
        {
            Vector3 startRot = transform.localEulerAngles;
            transform.LookAt(currentEnemy ? currentEnemy.transform : currentTarget);
            transform.localEulerAngles = new Vector3(startRot.x, transform.localEulerAngles.y, 0);
        }

        protected override string Type()
        {
            return TypesEnemies.Bred;
        }
    }
}