using System.Collections.Generic;

using UnityEngine;

namespace Society.Enemies
{
    internal sealed class BredEnemy : Enemy
    {
        [SerializeField] private List<Rigidbody> allJoints = new List<Rigidbody>();
        protected override void Start()
        {
            base.Start();
            deathClip = Resources.LoadAll<AudioClip>("Enemyes\\Death\\Bred\\");
            MakePhysical(false);
        }
        protected override void Death(float health)
        {
            if (health > UniqueVariables.MinHealth)
                return;

            mAgent.enabled = false;
            MakePhysical(true);
            enabled = false;
            DeathEvent.Invoke();
            PlayerSoundReceiversCollection.RemoveListner(this);
            stepEnemy.PlayDeathClip(deathClip);
        }
        private void MakePhysical(bool v)
        {
            mAnim.enabled = !v;

            foreach (var j in allJoints)
                j.isKinematic = !v;
        }

        private void OnValidate()
        {
            allJoints = new List<Rigidbody>();
            foreach (var c in GetComponentsInChildren<CharacterJoint>())
            {
                allJoints.Add(c.GetComponent<Rigidbody>());
            }
        }

        protected override void RotateBodyToTarget()
        {
            var direction = (possibleTargetPos - transform.position).normalized;
            direction.y = 0f;

            if ((mAgent.steeringTarget - transform.position) != Vector3.zero)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 1);
        }
    }
}