using System;
using System.Collections;

using Society.Patterns;

using UnityEngine;

namespace Society.Features.Sink
{
    public class SinkInteractiveObject : InteractiveObject
    {
        public event Action FinishProcedureEvent;

        [SerializeField] private GameObject particleSystemEffect;

        [SerializeField] private AudioSource mAudioSource;
        [SerializeField] private AudioClip procedureClip;
        public override void Interact()
        {
            particleSystemEffect.SetActive(true);

            StartCoroutine(nameof(Procedure));
        }

        private IEnumerator Procedure()
        {
            mAudioSource.clip = procedureClip;
            mAudioSource.Play();

            yield return new WaitForSeconds(5);

            mAudioSource.Stop();
            particleSystemEffect.SetActive(false);
            FinishProcedureEvent?.Invoke();
        }
    }
}