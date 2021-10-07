using UnityEngine;
namespace Society.Enemies
{
    sealed class DogEnemy : Enemy
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
            SetAnimationClip();
            mAnim.applyRootMotion = true;
            enabled = false;
            DeathEvent.Invoke();
            EnemiesData.RemoveEnemy(this);
            PlayDeathClip();
        }
    }
}