using System.Collections;
using UnityEngine;

namespace Anomaly_1
{
    class AnomalyPulsation : MonoBehaviour
    {
        [SerializeField] private float lightningStartDelay; // "StartDelay" из Lightning (ParticleSystem).
        [SerializeField] private float anomalyBoxSide; // Сторона куба опасной зоны аномалии.
        [SerializeField] private float damagePower;
        [SerializeField] private float radiationPower;
        private Collider playerCollider;
        private PlayerClasses.BasicNeeds playerBasicNeeds;
        private AudioSource mAud;
        private AudioClip PulsateClip;

        private void Awake()
        {
            FindObjectOfType<FirstPersonController>().TryGetComponent(out playerCollider);
            playerCollider.TryGetComponent(out playerBasicNeeds);
            mAud = gameObject.AddComponent<AudioSource>();
            mAud.spatialBlend = 1;
            PulsateClip = Resources.Load<AudioClip>("Anomalyes\\An_0");
        }
        public void Pulsate(Vector3 spawnPosition)
        {
            mAud.PlayOneShot(PulsateClip);
            StartCoroutine(PrepareForPulsation(spawnPosition));            
        }

        private IEnumerator PrepareForPulsation(Vector3 spawnPosition)
        {
            yield return new WaitForSeconds(lightningStartDelay);
            Affect(spawnPosition);
        }

        private void Affect(Vector3 spawnPosition)
        {
            var colliders = Physics.OverlapBox(spawnPosition, new Vector3(0.5f * anomalyBoxSide, 0.5f * anomalyBoxSide, 0.5f * anomalyBoxSide));
            foreach (var collider in colliders)
            {
                if (collider.Equals(playerCollider))
                    playerBasicNeeds.InjurePerson(GetRandomPower(damagePower), GetRandomPower(radiationPower));
                else if(collider.TryGetComponent<EnemyCollision>(out var ec))
                {
                    ec.InjureEnemy(GetRandomPower(damagePower), false);
                }
            }
        }
        private float GetRandomPower(float defValue)
        {
            return Random.Range(0.75f, 1.25f) * defValue;
        }
    }
}