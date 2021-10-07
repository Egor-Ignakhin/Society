using Society.Inventory;
using Society.Patterns;

using System.Collections.Generic;

using UnityEngine;
namespace Features.RandomBoxes
{
    public abstract class RandomlyBox : InteractiveObject
    {
        [SerializeField] protected List<RandomlyDropeedItem> randomlyDroppedItems = new List<RandomlyDropeedItem>();
        [SerializeField] private Vector3 lockedEulers;
        [SerializeField] private Vector3 openedEulers;
        protected bool IsOpened
        {
            get => isOpened; set
            {
                UpdateType(value);
                isOpened = value;
            }
        }
        private InventoryEventReceiver inventoryEventReceiver;
        private InventoryContainer inventoryContainer;
        private bool isOpened;
        private static MovableAudioSource movableAudioSource;
        protected enum InveractionType { OnOpened, OnLocked }

        private void Start()
        {
            inventoryContainer = FindObjectOfType<InventoryContainer>();
            inventoryEventReceiver = inventoryContainer.EventReceiver;
            IsOpened = false;
            OnInit();
        }
        protected abstract void OnInit();
        public override void Interact()
        {
            if (IsOpened)
                return;

            var sc = inventoryEventReceiver.GetSelectedCell();
            if (!sc)
            {
                PlayAudClipByState(InveractionType.OnLocked);
                return;
            }
            foreach (var (id, isConsumable) in UnlockItems)
            {
                if (inventoryEventReceiver.GetSelectedCell().Id == id)
                {
                    int it = Random.Range(0, randomlyDroppedItems.Count);
                    inventoryContainer.AddItem((int)randomlyDroppedItems[it].Id, randomlyDroppedItems[it].Count, randomlyDroppedItems[it].Gun);
                    IsOpened = true;
                    if (isConsumable)
                        inventoryEventReceiver.GetSelectedCell().DelItem(1);

                    PlayAudClipByState(InveractionType.OnOpened);

                    return;
                }
            }
            PlayAudClipByState(InveractionType.OnLocked);
        }
        private void PlayAudClipByState(InveractionType inveractionType)
        {
            if (movableAudioSource == null)
                movableAudioSource = new MovableAudioSource();


            movableAudioSource.PlayClip(playerInteractive.GetHitPoint(), clipsByIT[inveractionType]);
        }

        protected List<(int id, bool isConsumable)> UnlockItems;
        protected Dictionary<InveractionType, AudioClip[]> clipsByIT;

        protected abstract void UpdateType(bool value);
        protected void SetRotationAtType(bool value) =>
            transform.localEulerAngles = value ? openedEulers : lockedEulers;

        private void OnValidate()
        {
            foreach (var r in randomlyDroppedItems)
            {
                r.Count = Mathf.Clamp(r.Count, 1, ItemStates.GetMaxCount((int)r.Id));
            }
        }

    }
    [System.Serializable]
    public class RandomlyDropeedItem
    {
        public ItemStates.ItemsID Id;
        public int Count;
        public SMGInventoryCellGun Gun;
    }
    /// <summary>
    /// Передвижной источник аудио для <see cref="RandomlyBox"/>
    /// Этим можно добиться оптимизации.
    /// </summary>
    public class MovableAudioSource
    {
        private readonly AudioSource mAudioSource;
        private readonly GameObject mGO;
        private readonly Transform mTransform;
        public MovableAudioSource()
        {
            mGO = new GameObject(nameof(MovableAudioSource));
            mTransform = mGO.transform;
            mAudioSource = mGO.AddComponent<AudioSource>();
            mAudioSource.spatialBlend = 1;
            mAudioSource.rolloffMode = AudioRolloffMode.Linear;
            mAudioSource.minDistance = 0.1f;
            mAudioSource.volume = 0.15f;
        }
        public void PlayClip(Vector3 position, AudioClip[] sourceClip)
        {
            mTransform.position = position;
            mAudioSource.Stop();
            int numeratorClips = Random.Range(0, sourceClip != null ? sourceClip.Length : 0);
            mAudioSource.clip = sourceClip.Length > 0 ? sourceClip[numeratorClips] : null;
#if UNITY_EDITOR
            if (mAudioSource.clip == null)
                Debug.Log($"Clip on {nameof(RandomlyBox)} is null!");
#endif
            mAudioSource.Play();
        }
    }
}