using UnityEngine;
namespace Society.Enemies
{
    internal sealed class DogEnemy : Enemy
    {
        protected override void Start()
        {
            base.Start();
            deathClip = Resources.LoadAll<AudioClip>("Enemyes\\Death\\BloodDog\\");
        }
        protected override void Death(float health)
        {
            if (health > UniqueVariables.MinHealth)
                return;
            mAgent.enabled = false;
            SetAnimationClip(AnimationsContainer.None);
            mAnim.applyRootMotion = true;
            enabled = false;
            DeathEvent.Invoke();
            PlayerSoundReceiversCollection.RemoveListner(this);
            stepEnemy.PlayDeathClip(deathClip);
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