using Society.Player.Controllers;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Society.Anomalies.Carousel
{
    public class CarouselManager : MonoBehaviour
    {
        private readonly List<Rigidbody> items = new List<Rigidbody>();
        private Collider mCollider;
        private Vector3 PointPos;
        private Vector3 targetPos = Vector3.zero;
        private BoxCollider zone;
        [SerializeField] private GameObject explosion;
        private readonly float explosionImpulse = 20;
        [SerializeField] private GameObject piece;
        [SerializeField] [Range(1, 10)] private int health;
        private int Health
        {
            get { return health; }
            set
            {
                health = value;
                if (value == 0) Die();
            }
        }

        #region Player Interaction

        #region  Variables

        #region Mechanics
        [Range(0f, 10f)] [SerializeField] private float interactionTime = 2;
        [Range(0f, 20f)] [SerializeField] private float playerMaxHeight = 2;
        [Range(0f, 180f)] [SerializeField] private float degreesPerSecondMax = 90;
        private float degreesPerSec = 0;
        private GameObject player;
        private FirstPersonController playerFPC;
        private Vector3 playerAcceleration;
        #endregion

        #region State Pattern
        private Dictionary<Type, ICarouselBehaviour> behavioursMap;
        private ICarouselBehaviour currentBehaviour;
        #endregion

        #endregion

        #region Methods

        public void OnBulletEnter()
        {
            Health--;
        }
        private void Die()
        {
            int piecesNum = UnityEngine.Random.Range(1, 4);
            for (int i = 0; i < piecesNum; i++)
            {
                GameObject p = Instantiate(piece, PointPos, Quaternion.identity);
                p.GetComponent<CarouselePiece>().AddImpulse(Quaternion.AngleAxis(120 * i, Vector3.up) * Vector3.forward * explosionImpulse);
            }
            Instantiate(explosion, PointPos, Quaternion.identity);
            gameObject.SetActive(false);
        }
        #region State Pattern Methods
        private void InitBehaviours()
        {
            behavioursMap = new Dictionary<Type, ICarouselBehaviour>();
            behavioursMap[typeof(SearchPlayer)] = new SearchPlayer(this, interactionTime);
            behavioursMap[typeof(HoldPlayer)] = new HoldPlayer(this, interactionTime);
            behavioursMap[typeof(FreePlayer)] = new FreePlayer(this, interactionTime);
        }
        private void SetBehaviour(ICarouselBehaviour newBehaviour)
        {
            currentBehaviour = newBehaviour;
            currentBehaviour.Enter();
        }
        private ICarouselBehaviour GetBehaviour<T>() where T : ICarouselBehaviour
        {
            var type = typeof(T);
            return behavioursMap[type];
        }
        private void SetBehaviourByDefault()
        {
            SetBehaviourSearch();
        }
        public void SetBehaviourSearch()
        {
            var behaviour = GetBehaviour<SearchPlayer>();
            SetBehaviour(behaviour);
        }
        public void SetBehaviourHold()
        {
            var behaviour = GetBehaviour<HoldPlayer>();
            SetBehaviour(behaviour);
        }
        public void SetBehaviourFree()
        {
            var behaviour = GetBehaviour<FreePlayer>();
            SetBehaviour(behaviour);
        }
        #endregion

        #region Mechanics Methods
        public void SetPlayer(GameObject player)
        {
            this.player = player;
        }
        public Transform GetPlayerTransform()
        {
            return player.transform;
        }
        public Vector3 GetPointPos()
        {
            return PointPos;
        }
        public void SetPlayerFPC()
        {
            playerFPC = player.GetComponent<FirstPersonController>();
        }
        public void PlayerFixedUpdate()
        {
            if (degreesPerSec == 0) StartCoroutine(DegreesPerSecCalc());
            if (player != null)
            {
                //move:
                playerAcceleration.y += 0.15f * (-player.GetComponent<Rigidbody>().velocity.y) + 0.08f * (playerMaxHeight - player.transform.position.y);
                player.GetComponent<Rigidbody>().AddForce(playerAcceleration * player.GetComponent<Rigidbody>().mass);
                //rotate:
                playerFPC.AdditionalXMouse = degreesPerSec / 500; //не является точным значением в град/сек, ориентировочно
            }

        }

        System.Collections.IEnumerator DegreesPerSecCalc()
        {
            float times = interactionTime * 10;
            for (int i = 0; i < times; i++)
            {
                degreesPerSec += (degreesPerSecondMax / times);
                yield return new WaitForSeconds(0.1f);
            }
            degreesPerSec = 0;
            playerFPC.AdditionalXMouse = 0;
            yield break;
        }
        #endregion

        #endregion

        #endregion

        public void InitCollider(Collider c)
        {
            mCollider = c;
            PointPos = mCollider.transform.position;
            PointPos.y = (mCollider as CapsuleCollider).height;
        }

        private System.Collections.IEnumerator Start()
        {
            InitBehaviours();
            SetBehaviourByDefault();
            playerAcceleration = new Vector3(0, 0, 0);
            while (true)
            {
                targetPos = CalculateSpawnPosition(zone.transform, zone);
                yield return new WaitForSeconds(4);
            }
        }
        internal void SetZone(BoxCollider mCollider) => zone = mCollider;

        public Vector3 CalculateSpawnPosition(Transform transform, Collider collider)
        {
            var width = collider.bounds.size.x;
            var center = transform.position;
            return new Vector3(GetRandomDot(center.x, width, 0),
                                0,
                                GetRandomDot(center.z, width, 0));
        }
        private float GetRandomDot(float center, float sideLength, float axisLowestLimit)
        {
            var LowestVertex = center + axisLowestLimit - (sideLength / 2);
            var HeighestVertex = center + (sideLength / 2);
            var RandomDot = UnityEngine.Random.Range(LowestVertex, HeighestVertex);
            return RandomDot;
        }

        public void AddInList(Rigidbody rb) => items.Add(rb);

        public void RemoveFromList(Rigidbody rb) => items.Remove(rb);

        private void FixedUpdate()
        {
            Vector3 itemPos;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].gameObject.GetComponent<FirstPersonController>() != null)
                {
                    if (player == null) currentBehaviour.PlayerDetected(items[i].gameObject);
                }
                else
                {
                    itemPos = items[i].transform.position;
                    float m = Vector3.Distance(PointPos, itemPos);
                    m *= 1 / m;
                    items[i].AddForce((PointPos - itemPos).normalized * m, ForceMode.VelocityChange);
                    items[i].angularVelocity += new Vector3(5, 0, 0) * Time.fixedDeltaTime;
                    if (itemPos.y > (PointPos.y - 1))
                    {
                        items[i].AddForce(-(PointPos - itemPos).normalized * m * 100, ForceMode.VelocityChange);
                    }
                }

            }
            currentBehaviour.Update();
            MoveToTarget();
        }
        private void MoveToTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.fixedDeltaTime);
        }
    }
    #region State classes
    public interface ICarouselBehaviour
    {
        void Enter();
        void Exit();
        void Update();
        void PlayerDetected(GameObject p);
    }

    public class HoldPlayer : ICarouselBehaviour
    {
        private CarouselManager cm;
        private float t;
        public void PlayerDetected(GameObject p) {/*do nothing*/}
        public HoldPlayer(CarouselManager cm, float t)
        {
            this.cm = cm;
            this.t = t;
        }
        public void Enter()
        {
            cm.StartCoroutine(Timer());
        }
        public void Exit()
        {
            cm.SetBehaviourFree();
        }
        public void Update()
        {
            cm.PlayerFixedUpdate();
        }
        System.Collections.IEnumerator Timer()
        {
            yield return new WaitForSeconds(t);
            Exit();
            yield break;
        }
    }
    public class FreePlayer : ICarouselBehaviour
    {
        private CarouselManager cm;
        private float t;
        public void PlayerDetected(GameObject p) {/*do nothing*/}
        public FreePlayer(CarouselManager cm, float t)
        {
            this.cm = cm;
            this.t = t;
        }
        public void Enter()
        {
            cm.SetPlayer(null);
            cm.StartCoroutine(Timer());
        }
        public void Exit()
        {
            cm.SetBehaviourSearch();
        }
        public void Update()
        {

        }
        System.Collections.IEnumerator Timer()
        {
            yield return new WaitForSeconds(t);
            Exit();
            yield break;
        }
    }
    public class SearchPlayer : ICarouselBehaviour
    {
        private CarouselManager cm;
        public void PlayerDetected(GameObject p)
        {
            cm.SetPlayer(p);
            cm.SetPlayerFPC();
            Exit();
        }
        public SearchPlayer(CarouselManager cm, float t)
        {
            this.cm = cm;
        }
        public void Enter()
        {

        }
        public void Exit()
        {
            cm.SetBehaviourHold();
        }
        public void Update()
        {

        }
    }
    #endregion
}