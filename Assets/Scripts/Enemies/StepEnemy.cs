using Society.Effects;

using UnityEngine;

namespace Society.Enemies
{
    public sealed class StepEnemy : StepPlayer
    {
        private readonly Enemy enemy;
        public StepEnemy(IMovableController e, StepSoundData ssd)
        {
            stepSoundData = ssd;

            enemy = (Enemy)e;
            enemy.EnemyStepEvent += OnStep;

            stepPlayerSource = enemy.gameObject.AddComponent<AudioSource>();
            stepPlayerSource.priority = 129;
            stepPlayerSource.spatialBlend = 1;
            stepPlayerSource.pitch = Random.Range(0.95f, 1.05f);
        }
        public void OnDestroy()
        {
            enemy.EnemyStepEvent -= OnStep;
        }

        internal void PlayDeathClip(AudioClip[] deathClip)
        {
            int index = Random.Range(0, deathClip.Length);
            stepPlayerSource.PlayOneShot(deathClip[index]);
        }
    }
}