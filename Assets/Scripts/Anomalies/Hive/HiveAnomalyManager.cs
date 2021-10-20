
using Society.Enemies;
using Society.Player;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
namespace Society.Anomalies.Hive
{
    internal sealed class HiveAnomalyManager : Anomaly, IPlayerSoundReceiver
    {
        [SerializeField] private HiveEnemy hiveBeeEnemyInstance;
        [SerializeField] private BoxCollider boxColliderSpawnRange;
        [SerializeField] private float yLowestLimit; // Ниже это высоты аномалия не будет появляться.        

        private Transform player;
        private readonly List<HiveEnemy> hiveBeeList = new List<HiveEnemy>();
        private BasicNeeds playerBn;
        [SerializeField] private Inventory.InventoryItem hiveBeeArtefactInstance;

        internal void Hit()
        {
            health--;

            if (health <= 0)
                OnDie();
        }
        private void Awake()
        {
            PlayerSoundReceiversCollection.AddListner(this);
            player = FindObjectOfType<Player.Controllers.FirstPersonController>().transform;

            StartCoroutine(nameof(GenerateBee));
            StartCoroutine(nameof(AttackPlayer));
        }

        private void Update()
        {
            if (playerBn)
            {
                foreach (var bee in hiveBeeList)
                {
                    bee.SetTargetPosition(RecalculateTargetPosition());
                }
            }
        }

        private IEnumerator GenerateBee()
        {
            while (true)
            {
                if (hiveBeeList.Count < 10)
                {
                    hiveBeeList.Add(Instantiate(hiveBeeEnemyInstance, transform.position, Quaternion.identity, transform));
                    hiveBeeList[hiveBeeList.Count - 1].OnInit(this);
                }
                yield return new WaitForSeconds(3);
            }
        }

        internal Vector3 RecalculateTargetPosition() => playerBn ? player.position :
                Extensions.CalculateSpawnPositionInRange(boxColliderSpawnRange.transform, boxColliderSpawnRange);       

        public Transform GetTransform() => transform;

        public float GetDistanceToTarget() => Vector3.Distance(transform.position, player.position);

        public void SetPlayer(BasicNeeds bn)
        {
            playerBn = bn;
        }
        public void OnDie()
        {
            PlayerSoundReceiversCollection.RemoveListner(this);

            gameObject.SetActive(false);

            Instantiate(hiveBeeArtefactInstance, transform.position, Quaternion.identity);
        }
        private IEnumerator AttackPlayer()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                if (!playerBn)
                    continue;

                foreach (var bee in hiveBeeList)
                {
                    if (Vector3.Distance(bee.transform.position, player.position) > bee.AttackDistance())
                        continue;

                    playerBn.InjurePerson(bee.Damage());
                }
            }
        }
    }
}