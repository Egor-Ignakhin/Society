using UnityEngine;
using UnityEngine.AI;

namespace EnviromentMobs
{
    sealed class ShrummiExample : PoolableObject
    {
        private Animator mAnim;
        private NavMeshAgent mAgent;
        private readonly Transform target;

        public void OnInit()
        {
            mAnim = GetComponent<Animator>();
            mAgent = GetComponent<NavMeshAgent>();            
        }

        private void FixedUpdate()
        {
            return;
            mAnim.SetBool("IsMove", mAgent.isOnNavMesh);
            if (mAgent.isOnNavMesh)
            {
                mAgent.SetDestination(target.position);
            }
            if ((mAgent.steeringTarget - transform.position) != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(mAgent.steeringTarget - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);

                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            }

        }
    }
}