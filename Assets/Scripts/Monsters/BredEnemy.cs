using System.Collections.Generic;

using UnityEngine;

namespace Enemies
{
    sealed class BredEnemy : Enemy
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
            MonstersData.RemoveEnemy(this);
            PlayDeathClip();
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
    }
}