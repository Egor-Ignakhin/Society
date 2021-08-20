using UnityEngine;
namespace Missions
{
    sealed class TriggerTaskChecker : MissionInteractiveObject
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