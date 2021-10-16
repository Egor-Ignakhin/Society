
using Society.Enemies;
using Society.Player;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
namespace Society.Anomalies.Apiary
{
    internal sealed class ApiaryManager : MonoBehaviour, IPlayerSoundReceiver
    {
        [SerializeField] private ApiaryBee beeInstansce;
        [SerializeField] private BoxCollider boxColliderSpawnRange;
        [SerializeField] private float yLowestLimit; // Ниже это высоты аномалия не будет появляться.

        private float health = 2;
        internal void Hit()
        {
            health--;

            if (health <= 0)
                OnDie();
        }        

        private Transform player;
        private List<ApiaryBee> beesList = new List<ApiaryBee>();
        private BasicNeeds playerBn;
        [SerializeField] private Inventory.InventoryItem artefactInstance;

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
                foreach (var bee in beesList)
                {
                    bee.SetTargetPosition(RecalculateTargetPosition());
                }
            }
        }

        private IEnumerator GenerateBee()
        {
            while (true)
            {
                if (beesList.Count < 10)
                {
                    beesList.Add(Instantiate(beeInstansce, transform.position, Quaternion.identity, transform));
                    beesList[beesList.Count - 1].OnInit(this);
                }
                yield return new WaitForSeconds(3);
            }
        }

        internal Vector3 RecalculateTargetPosition()
        {
            if (playerBn)
            {
                return player.position;
            }
            else return CalculateSpawnPosition();
        }

        public Vector3 CalculateSpawnPosition()
        {
            var width = boxColliderSpawnRange.bounds.size.x;
            var height = boxColliderSpawnRange.bounds.size.y;
            var center = boxColliderSpawnRange.transform.position;
            return new Vector3(GetRandomDot(center.x, width, 0),
                                GetRandomDot(center.y, height, yLowestLimit),
                                GetRandomDot(center.z, width, 0));
        }

        private float GetRandomDot(float center, float sideLength, float axisLowestLimit)
        {
            var LowestVertex = center + axisLowestLimit - (sideLength / 2);
            var HeighestVertex = center + (sideLength / 2);
            var RandomDot = Random.Range(LowestVertex, HeighestVertex);
            return RandomDot;
        }

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

            Instantiate(artefactInstance, transform.position, Quaternion.identity);
        }
        private IEnumerator AttackPlayer()
        {
            while (true)
            {
                if (playerBn)
                {
                    foreach (var bee in beesList)
                    {
                        if (Vector3.Distance(bee.transform.position, player.position) <= bee.AttackDistance())
                        {
                            playerBn.InjurePerson(bee.Damage());
                        }
                    }
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}