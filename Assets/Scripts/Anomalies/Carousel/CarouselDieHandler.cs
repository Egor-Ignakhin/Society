using System.Collections.Generic;

using UnityEngine;

namespace Society.Anomalies.Carousel
{
    public sealed class CarouselDieHandler : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> tornadoParticleSystem;
        [SerializeField] private GameObject carouselManager;
        [SerializeField] private Animator carouselAuraAnimator;
        private GameObject Piece;

        private bool isDie = false;
        private bool canDestroyArtefact;
        private int piecesNum;
        private Vector3 PointPos;
        private float ExplosionImpulse;
        private float timerDissolveDestroyArtefact = 5;

        public void OnDie(int piecesNum, Vector3 pointPos, float explosionImpulse, GameObject piece)
        {
            isDie = true;
            this.piecesNum = piecesNum;
            this.PointPos = pointPos;
            this.ExplosionImpulse = explosionImpulse;
            this.Piece = piece;
        }
        private void Update()
        {
            if (!isDie)
                return;

            if (canDestroyArtefact)
            {
                DestroyArtefact();

                canDestroyArtefact = false;
                gameObject.SetActive(false);
            }

            for (int i = 0; i < tornadoParticleSystem.Count; i++)
            {
                var tpsEmission = tornadoParticleSystem[i].emission;
                tpsEmission.rateOverTime = Mathf.MoveTowards(tpsEmission.rateOverTime.constant, 0, Time.deltaTime * 10);
            }

            if ((timerDissolveDestroyArtefact -= Time.deltaTime) <= 0)
                canDestroyArtefact = true;
        }

        private void DestroyArtefact()
        {
            carouselAuraAnimator.enabled = true;
        }
        public void DropPiece()
        {            
            for (int i = 0; i < piecesNum; i++)
            {
                GameObject p = Instantiate(Piece, PointPos, Quaternion.identity);
                var cpII = p.GetComponent<CarouselePieceII>();
                cpII.EnableParticleEffect();
                cpII.AddImpulse(Quaternion.AngleAxis(120 * i, Vector3.up) * Vector3.forward * ExplosionImpulse);
            }
        }
    }
}