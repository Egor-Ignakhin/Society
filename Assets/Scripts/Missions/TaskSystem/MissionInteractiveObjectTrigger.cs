using Society.Player.Controllers;

using UnityEngine;
namespace Society.Missions.TaskSystem
{
    internal sealed class MissionInteractiveObjectTrigger : MissionInteractiveObject
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