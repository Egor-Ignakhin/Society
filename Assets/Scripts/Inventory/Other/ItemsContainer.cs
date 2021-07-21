using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    /// <summary>
    /// класс-контейнер (сундук, ящик и тп)
    /// </summary>
    public sealed class ItemsContainer : InteractiveObject
    {
        public const int maxCells = 40;
        [HideInInspector] [SerializeField] private Types startedType;
        [HideInInspector] [Range(0, maxCells)] [SerializeField] private int cellsCount;
        private List<(int id, int count, SMGInventoryCellGun gun)> container = new List<(int id, int count, SMGInventoryCellGun gun)>();
        public List<(int id, int count, SMGInventoryCellGun gun)> GetData() => container;
        private bool isOpened;
        private InventoryEventReceiver inventoryEventReceiver;
        [HideInInspector] public List<int> StartedItems = new List<int>();
        [HideInInspector] public List<int> StartedCount = new List<int>();

        #region SMG
        [HideInInspector] public List<int> Aims = new List<int>();
        [HideInInspector] public List<int> Mags = new List<int>();
        [HideInInspector] public List<int> Silencers = new List<int>();
        #endregion
        public (List<int> items, List<int> count, List<int> aims, List<int> mags, List<int> silencers) GetStartedData() => (StartedItems, StartedCount, Aims, Mags, Silencers);

        private AudioSource mAud;
        private AudioClip OpenCloseClip;
        private void Start()
        {
            inventoryEventReceiver = FindObjectOfType<InventoryContainer>().EventReceiver;
            for (int i = 0; i < StartedItems.Count; i++)
            {
                var possibleGun = new SMGInventoryCellGun();
                possibleGun.Reload(StartedItems[i], Mags[i], Silencers[i], 0, Aims[i]);
                container.Add((StartedItems[i], StartedCount[i], possibleGun));
            }
            SetType(startedType.ToString());
            mAud = gameObject.AddComponent<AudioSource>();
            mAud.spatialBlend = 1;
            mAud.rolloffMode = AudioRolloffMode.Linear;
            OpenCloseClip = Resources.Load<AudioClip>("Boxes\\OpenClose\\_0");
        }

        public override void Interact()
        {
            if (isOpened)
                return;
            inventoryEventReceiver.OpenContainer(container, cellsCount, this);
            isOpened = true;
            mAud.PlayOneShot(OpenCloseClip);
        }
        /// <summary>
        /// метод закрывает сохраняет ячейки в памяти
        /// </summary>
        public void Close(List<(int id, int count, SMGInventoryCellGun gun)> lst)
        {
            container = lst;
            isOpened = false;
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            startedType = Types.Container_1;

            if (StartedItems.Count < cellsCount)
            {
                for (int i = StartedItems.Count; i < cellsCount; i++)
                {
                    StartedItems.Add(0);
                    StartedCount.Add(1);
                    Silencers.Add(0);
                    Mags.Add(0);
                    Aims.Add(0);
                }
            }
            else
            {
                while (StartedItems.Count > cellsCount)
                {
                    StartedItems.RemoveAt(StartedItems.Count - 1);
                    StartedCount.RemoveAt(StartedCount.Count - 1);
                    Silencers.RemoveAt(Silencers.Count - 1);
                    Mags.RemoveAt(Mags.Count - 1);
                    Aims.RemoveAt(Aims.Count - 1);
                }
            }
            //фикс возможных проблем связанных с "количеством пустого слота"
            for (int i = 0; i < StartedItems.Count; i++)
            {
                if ((StartedCount[i] > 0) &&
                    (StartedItems[i] == 0))
                {
                    StartedCount[i] = 0;
                    Silencers[i] = 0;
                    Mags[i] = 0;
                    Aims[i] = 0;
                }
            }
            //фикс когда количество обычных предметов = 0
            for (int i = 0; i < StartedItems.Count; i++)
            {
                if ((StartedItems[i] > 0) &&
                    (StartedCount[i] == 0))
                {
                    StartedCount[i] = 1;
                    {
                        StartedCount[i] = 0;
                        Silencers[i] = 0;
                        Mags[i] = 0;
                        Aims[i] = 0;
                    }
                }
            }
        }

        public void RemoveStartedItem(int index)
        {
            StartedItems.RemoveAt(index);
            StartedCount.RemoveAt(index);
            Mags.RemoveAt(index);
            Silencers.RemoveAt(index);
            Aims.RemoveAt(index);
            cellsCount--;
        }

        private void Reset()
        {
            startedType = Types.Container_1;
            StartedCount.Clear();
            StartedItems.Clear();

            Aims.Clear();
            Mags.Clear();
            Silencers.Clear();

            cellsCount = 0;
            Debug.ClearDeveloperConsole();
        }

        public void SetStartedSilencerIndex(int index, int newValue) =>
            Silencers[index] = Mathf.Clamp(newValue, 0, SMG.GunCharacteristics.GetMaxSilencerFromID(StartedItems[index]));

        public void SetStartedMagIndex(int index, int newValue) =>
            Mags[index] = Mathf.Clamp(newValue, 0, SMG.GunCharacteristics.GetMaxMagFromID(StartedItems[index]));

        public void SetStartedAimIndex(int index, int newValue) =>
            Aims[index] = Mathf.Clamp(newValue, 0, SMG.GunCharacteristics.GetMaxAimFromID(StartedItems[index]));


        public void AddStartedItem(int itemID)
        {
            StartedItems.Add(itemID);

            StartedCount.Add(1);

            Silencers.Add(0);
            Mags.Add(0);
            Aims.Add(0);

            cellsCount++;
        }

        public void SetStartedCount(int index, int newCount) =>
            StartedCount[index] = newCount;
#endif
    }
}