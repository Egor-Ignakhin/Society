using System.Collections.Generic;
using UnityEngine;

namespace Debugger
{
    class DebugManager : MonoBehaviour
    {
        [SerializeField] private Transform background;// фон
        private bool isHidden = true;
        private bool isMoving = false;
        [SerializeField] private Vector3 hiddenPos;
        [SerializeField] private Vector3 ShowingPos;
        private IDebug activeDebugger;
        [SerializeField] private List<GameObject> DebuggersObjects = new List<GameObject>();// лист дебаггеров-частиц
        private readonly List<IDebug> Debuggers = new List<IDebug>();// тот же лист но с уже взятым от частиц интерфейсом
        private void Awake()
        {
            foreach (var d in DebuggersObjects)// заполнение листа частиц
            {
                Debuggers.Add(d.GetComponent<IDebug>());
            }

            DisableDebuggers();
            activeDebugger = Debuggers[0];
        }
        private void Update()
        {
            if (isMoving)
                Move();
            if (Input.GetKeyDown(KeyCode.F1))
            {                
                isHidden = !isHidden;
                isMoving = true;                
            }
        }
        /// <summary>
        /// выключатель всех дебаггеров-частиц
        /// </summary>
        private void DisableDebuggers()
        {
            foreach (var d in Debuggers)
            {
                d.Active = false;
                d.gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// установка активного дебаггера
        /// </summary>
        /// <param name="ad"></param>
        public void SetActiveDebugger(GameObject ad)
        {
            DisableDebuggers();
            activeDebugger = ad.GetComponent<IDebug>();
            activeDebugger.Activate();
        }
        /// <summary>
        /// анимация движения
        /// </summary>
        private void Move()
        {
            InventoryInput.Instance.SimularActive(false);
            if (!isHidden)
            {
                InputManager.LockInput();
                InputManager.DisableInput();
            }
            else
            {
                InputManager.Unlock();
                InputManager.EnableInput();
            }

            Vector3 direction = isHidden ? hiddenPos : ShowingPos;// установка таргетной позиции

            if (background.localPosition != direction)// если анимация не кончилась
            {
                background.localPosition = Vector3.MoveTowards(background.localPosition, direction, 100);
                return;
            }


            background.gameObject.SetActive(!isHidden);           
            isMoving = false;
            activeDebugger.Activate();
        }
    }
}