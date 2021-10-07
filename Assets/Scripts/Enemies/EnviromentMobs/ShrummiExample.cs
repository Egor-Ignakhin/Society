using UnityEngine;
using UnityEngine.AI;

namespace Society.Monsters.EnviromentMobs
{
    public sealed class ShrummiExample : MonoBehaviour
    {
        private Vector3 targetPosition;
        private ShrummiesManager shrummiesManager;
        private NavMeshAgent mAgent;
        private AudioSource mAudioSource;
        private AudioClip stepClip;

        private void Awake()
        {
            mAgent = GetComponent<NavMeshAgent>();
            mAudioSource = GetComponent<AudioSource>();
            transform.root.TryGetComponent(out shrummiesManager);
            shrummiesManager.AddFreeExample(this);
            stepClip = Resources.Load<AudioClip>("Enemyes\\Shrummie\\Shrummie_walk");
        }
        internal void OnInit(Vector3 spawnedPosition, Vector3 targetPosition, ShrummiesManager shrummiesManager)
        {
            this.transform.position = spawnedPosition;
            this.targetPosition = targetPosition;
            this.shrummiesManager = shrummiesManager;
        }

        private void FixedUpdate()
        {
            MoveToTarget();
        }
        private void MoveToTarget()
        {
            if (!mAgent.isOnNavMesh)
                return;

            mAgent.SetDestination(targetPosition);
            var direction = (targetPosition - transform.position).normalized;
            direction.y = 0f;
            if ((mAgent.steeringTarget - transform.position) != Vector3.zero)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 1);

            if (mAgent.pathPending)
                return;

            if (Vector3.Distance(mAgent.pathEndPosition, transform.position) >= mAgent.stoppingDistance)
                return;

            if (mAgent.hasPath && mAgent.velocity.sqrMagnitude != 0f)
                return;

            shrummiesManager.HideExample(this);
        }
        public void StepEvent()
        {
            mAudioSource.PlayOneShot(stepClip);
        }
    }
}