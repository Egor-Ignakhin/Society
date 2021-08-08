using UnityEngine;

namespace Features
{
    sealed class ShowerLeverOnWall : InteractiveObject
    {
        private bool leverIsOpened = false;
        [SerializeField] private ShowerExample showerExample;

        public bool LeverIsOpened
        {
            get => leverIsOpened; set
            {
                var em = waterEffect.emission;
                em.enabled = value;
                SetType(!value ? "ClosedShowerLever" : "OpenedShowerLever");
                leverIsOpened = value;
            }
        }
        [SerializeField] private ParticleSystem waterEffect;
        private void Start()
        {
            LeverIsOpened = false;
        }

        public override void Interact()
        {
            LeverIsOpened = !LeverIsOpened;
        }
        private void Update()
        {
            if (LeverIsOpened && showerExample.IsReloading)
            {
                showerExample.AddContentByTime();
            }
        }

    }
}