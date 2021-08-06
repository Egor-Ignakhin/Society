using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnviromentMobs
{
    sealed class Mut : MonoBehaviour
    {
        private Animator mAnim;
        private NavMeshAgent mAgent;
        private Transform player;

        private void Start()
        {
            mAnim = GetComponent<Animator>();
            mAgent = GetComponent<NavMeshAgent>();
            player = FindObjectOfType<FirstPersonController>().transform;
        }
        private void FixedUpdate()
        {
            mAnim.SetBool("IsMove", mAgent.isOnNavMesh);
            if (mAgent.isOnNavMesh)
            {
                mAgent.SetDestination(player.position);
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