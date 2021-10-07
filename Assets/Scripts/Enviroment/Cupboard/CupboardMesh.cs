using Society.Enviroment.Doors;
using Society.Patterns;

using UnityEngine;
namespace Society.Enviroment.Cupboard
{
    public sealed class CupboardMesh : InteractiveObject
    {
        [SerializeField] private CupboardManager mManager;
        public bool IsOpen { get; private set; }// открыт ли ящик
        private bool canInteract = true;// возможность взаимодействия
        [SerializeField] [Range(0, 10)] private float speed = 1;
        [SerializeField] private bool isLockedCupboard;
        [SerializeField] private DoorManager.OpenClipType clipType;
        [SerializeField] private DoorManager.OpenClipType lockType;
        private AudioSource mAud;
        private AudioClip openCloseClip;
        private AudioClip lockClip;
        protected override void Awake()
        {
            base.Awake();
            mManager.AddCase(this);
            SetOpened(IsOpen);
            mAud = GetComponent<AudioSource>();
            openCloseClip = Resources.Load<AudioClip>($"CuboardClips\\OpenClose\\{clipType}");
            lockClip = Resources.Load<AudioClip>($"CuboardClips\\Lock\\{lockType}");
        }
        public override void Interact()
        {
            if (!canInteract)
                return;
            if (isLockedCupboard)
            {
                mAud.PlayOneShot(lockClip);
                return;
            }
            mManager.Interact(transform, speed);
            canInteract = false;
            mAud.PlayOneShot(openCloseClip);
        }
        public void SetOpened(bool value)
        {
            IsOpen = value;
            canInteract = true;
            if (!isLockedCupboard)
                SetType(IsOpen ? "OpenedBox" : "ClosedBox");
            else
                SetType("LockedBox");
        }
    }
}