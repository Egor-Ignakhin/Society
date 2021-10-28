
using Society.Enemies;
using Society.Player;

using UnityEngine;

namespace Society.Anomalies.BallLighting
{
    internal sealed class BallLightingAnomalyDieHandler : AnomalyDieHandler
    {
        [SerializeField] private float radiusBlast;
        [SerializeField] private BallLightingAnomalyManager manager;
        public override void OnInit()
        {
            transform.SetParent(null);
            gameObject.SetActive(true);

            DoBlast();
        }

        private void DoBlast()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radiusBlast, ~0, QueryTriggerInteraction.Ignore);

            foreach (var c in colliders)
            {
                if (c.TryGetComponent(out EnemyCollision enemyCollision))
                {
                    enemyCollision.InjureEnemy(manager.GetBlastPower());
                }
                else if (c.TryGetComponent(out BasicNeeds bn))
                {
                    bn.InjurePerson(manager.GetBlastPower());
                }
            }
        }
    }
}