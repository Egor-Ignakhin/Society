using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace FriendlyPersons
{
    public class YanController : MonoBehaviour
    {
        private List<Transform> points = new List<Transform>();
        [SerializeField] private NavMeshAgent mAgent;

        private Transform targetPoint;

        private void Start()
        {
            foreach (var pp in GetComponentsInChildren<PersonsPoint>())
                points.Add(pp.transform);

            StartCoroutine(nameof(SetDistForAgent));
        }
        private IEnumerator SetDistForAgent()
        {
            mAgent.SetDestination((targetPoint = points[0]).position);
            while (true)
            {
                if (mAgent.isOnNavMesh)
                {
                    if (mAgent.remainingDistance <= (mAgent.stoppingDistance + 0.5f))
                    {
                        int iterator = points.IndexOf(targetPoint) + 1;
                        if (iterator >= points.Count)
                            iterator = 0;

                        mAgent.SetDestination((targetPoint = points[iterator]).position);
                    }
                    ThrowRay();
                }

                yield return new WaitForSeconds(1);
            }
        }
        private void ThrowRay()
        {
            Ray ray = new Ray(mAgent.transform.position, mAgent.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 1, ~0, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.TryGetComponent<DoorMesh>(out var door))
                {
                    door.Interact(null);
                }
            }
        }
        private void Update()
        {
            var targetRotation = Quaternion.LookRotation(mAgent.steeringTarget - mAgent.transform.position, Vector3.up);
            mAgent.transform.rotation = Quaternion.Slerp(mAgent.transform.rotation, targetRotation, Time.deltaTime * 2.0f);

            mAgent.transform.localEulerAngles = new Vector3(0, mAgent.transform.localEulerAngles.y, 0);
        }
    }
}