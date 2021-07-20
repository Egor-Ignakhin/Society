using UnityEngine;
namespace Missions
{
    sealed class TriggerTaskChecker : TaskChecker
    {
        private Collider playerCollider;
        protected override void Start()
        {
            base.Start();
            playerCollider = FindObjectOfType<FirstPersonController>().GetCollider();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other == playerCollider)
            {
                Report();
            }
        }
    }
}