using Society.Patterns;

using UnityEngine;
namespace Society.Enviroment.Doors
{
    public sealed class DoorMesh : InteractiveObject//класс реализует взаимодействие с дверью
    {
        [SerializeField] private DoorManager.RateTypes rateType;
        [ShowIf(nameof(rateType), DoorManager.RateTypes.extrim)] [SerializeField] private float lerpSpeed = 100;
        private DoorManager mManager;

        protected override void Awake()
        {
            base.Awake();
            mManager = transform.parent.GetComponent<DoorManager>();
            mManager.SetType(this);
            mManager.SetDefaultRate(rateType, lerpSpeed);
        }
        public override void Interact()
        {
            mManager.Interact(this);
        }
    }
}