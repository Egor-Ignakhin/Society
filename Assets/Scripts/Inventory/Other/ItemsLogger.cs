using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Inventory
{
    sealed class ItemsLogger : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loggerPrefab;
        private InventoryContainer inventoryContainer;

        private Stack<TextMeshProUGUI> disabledloggers;
        private List<TextMeshProUGUI> activeLoggers = new List<TextMeshProUGUI>();
        private const float delayToDisable = 4f;// длительность отображения логгера

        private void Start()
        {
            disabledloggers = new Stack<TextMeshProUGUI>();
            for (int i = 0; i < 5; i++)
                disabledloggers.Push(Instantiate(loggerPrefab, loggerPrefab.transform.position, Quaternion.identity, transform));


            foreach (var l in disabledloggers)
            {
                l.alpha = 0;
            }
            inventoryContainer = FindObjectOfType<InventoryContainer>();
            inventoryContainer.TakeItemEvent += DrawNewItem;
            inventoryContainer.ActivateItemEvent += DrawUsedItem;            
            inventoryContainer.MInventoryInput.DropEvent += DropItem;
        }
        private void Update()
        {
            for (int i = 0; i < activeLoggers.Count; i++)
            {
                if ((activeLoggers[i].alpha -= Time.deltaTime / delayToDisable) > 0)
                    continue;
                i++;
                disabledloggers.Push(activeLoggers[0]);
                activeLoggers.RemoveAt(0);
            }
        }

        /// <summary>
        /// отрисовка нового предмета
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        private void DrawNewItem(int id, int count)
        {
            if (!inventoryContainer.IsInitialized)
                return;

            var lg = AddNewLogger();
            lg.SetText($"Добавлено: {Localization.GetHint(id)}  x{count}");
            lg.color = Color.green;
        }
        private TextMeshProUGUI AddNewLogger()
        {
            TextMeshProUGUI activeLogger = null;
            if (disabledloggers.Count > 0)// если стек не пустой
                activeLoggers.Add(activeLogger = disabledloggers.Pop());

            if (!activeLogger)
            {
                disabledloggers.Push(activeLoggers[0]);
                activeLoggers.RemoveAt(0);
                activeLoggers.Add(activeLogger = disabledloggers.Pop());
            }
            activeLogger.transform.SetAsLastSibling();
            activeLogger.alpha = 1;

            return activeLogger;
        }
        /// <summary>
        /// отрисовка использованного предмета
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        private void DrawUsedItem(int id, int count)
        {
            if (!inventoryContainer.IsInitialized)
                return;
            var lg = AddNewLogger();
            lg.SetText($"Использовано: {Localization.GetHint(id)}  x{count}");
            lg.color = Color.white;
        }
        private void DropItem(string title, int count)
        {
            if (!inventoryContainer.IsInitialized)
                return;

            var lg = AddNewLogger();
            lg.SetText($"- {title}  x{count}");
            lg.color = Color.red;
        }
        private void OnDisable()
        {
            inventoryContainer.TakeItemEvent -= DrawNewItem;
            inventoryContainer.ActivateItemEvent -= DrawUsedItem;            
            inventoryContainer.MInventoryInput.DropEvent -= DropItem;
            disabledloggers = null;
            activeLoggers = null;
        }
    }
}