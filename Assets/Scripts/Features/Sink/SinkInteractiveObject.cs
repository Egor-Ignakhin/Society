using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Society.Patterns;

using UnityEngine;

namespace Society.Features.Sink
{
    public class SinkInteractiveObject : InteractiveObject
    {
        public event Action FinishProcedureEvent;

        [SerializeField] private GameObject particleSystemEffect;
        public override void Interact()
        {
            particleSystemEffect.SetActive(true);

            StartCoroutine(nameof(Procedure));
        }

        private IEnumerator Procedure()
        {
            yield return new WaitForSeconds(2);

            particleSystemEffect.SetActive(false);
            FinishProcedureEvent?.Invoke();
        }
    }
}