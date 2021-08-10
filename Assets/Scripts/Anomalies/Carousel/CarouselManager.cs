using System;
using System.Collections.Generic;
using UnityEngine;

namespace CarouselAnomaly
{
    public class CarouselManager : MonoBehaviour
    {
        private readonly List<Rigidbody> items = new List<Rigidbody>();
        private Collider mCollider;
        private Vector3 PointPos;
        private Vector3 targetPos = Vector3.zero;
        private BoxCollider zone;


        private GameObject player;
        private Dictionary<Type, ICarouselBehaviour> behavioursMap;
        private ICarouselBehaviour currentBehaviour;

        private void InitBehaviours()
        {
            behavioursMap = new Dictionary<Type, ICarouselBehaviour>();
            behavioursMap[typeof(SearchPlayer)] = new SearchPlayer(this);
            behavioursMap[typeof(HoldPlayer)] = new HoldPlayer(this);
            behavioursMap[typeof(FreePlayer)] = new FreePlayer(this);
        }
        private void SetBehaviour(ICarouselBehaviour newBehaviour)
        {
            //if (currentBehaviour != null)
                //currentBehaviour.Exit();
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
            SetBehaviour (behaviour);
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
        public void SetPlayer(GameObject player)
        {
            this.player = player;
        }









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
                if (items[i].gameObject.GetComponent<FirstPersonController>()!=null)
                {
                    Debug.Log("find");
                    if (player == null) currentBehaviour.PlayerDetected(items[i].gameObject);
                }
                else
                {
                    itemPos = items[i].transform.position;
                    float m = Vector3.Distance(PointPos, itemPos);
                    m *= 1 / m;
                    items[i].AddForce((PointPos - itemPos).normalized * m, ForceMode.VelocityChange);
                    items[i].angularVelocity += new Vector3(5, 0, 0) * Time.deltaTime;
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
        public void PlayerDetected(GameObject p) {/*do nothing*/}
        public HoldPlayer(CarouselManager cm)
        {
            this.cm = cm;
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
            Debug.Log("Up and rotate pl");
        }
        System.Collections.IEnumerator Timer()
        {
            yield return new WaitForSeconds(2);
            Exit();
            yield break;
        }
    }
    public class FreePlayer : ICarouselBehaviour
    {
        private CarouselManager cm;
        public void PlayerDetected(GameObject p) {/*do nothing*/}
        public FreePlayer(CarouselManager cm)
        {
            this.cm = cm;
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
            yield return new WaitForSeconds(2);
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
            Exit();
        }
        public SearchPlayer(CarouselManager cm)
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

}