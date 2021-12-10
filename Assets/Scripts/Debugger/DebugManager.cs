using System.Collections.Generic;

using Society.GameScreens;

using UnityEngine;

namespace Society.Debugger
{
    internal sealed class DebugManager : MonoBehaviour, IGameScreen
    {
        /// <summary>
        /// Фон
        /// </summary>
        [SerializeField] private Transform background;

        /// <summary>
        /// Окно скрыто?
        /// </summary>
        private bool isHidden = true;

        /// <summary>
        /// Окно движется(анимируется)?
        /// </summary>
        private bool isMoving = false;

        /// <summary>
        /// Позиция по Y в скрытом положении
        /// </summary>
        [SerializeField] private float hiddenPositionY;

        /// <summary>
        /// Позиция по Y в открытом положении
        /// </summary>
        [SerializeField] private float showingPositionY;

        /// <summary>
        /// Активный дебаггер
        /// </summary>
        private IDebug activeDebugger;

        /// <summary>
        /// Лист дебаггеров
        /// </summary>
        [SerializeField] private List<GameObject> debuggersObjects = new List<GameObject>();

        /// <summary>
        /// Тот же лист но с уже взятым от частиц интерфейсом
        /// </summary>
        private readonly List<IDebug> Debuggers = new List<IDebug>();

        /// <summary>
        /// Сущность отображаюшая текущий FPS пользователя в окне консоли
        /// </summary>
        [SerializeField] private FpsDrawer fpsDrawer;

        private void Awake()
        {
            // Заполнение листа дебаггеров
            foreach (var d in debuggersObjects)
            {
                Debuggers.Add(d.GetComponent<IDebug>());
            }

            DisableDebuggers();

            // Установка активным дебаггером нулевым в листе
            activeDebugger = Debuggers[0];
        }
        private void Update()
        {
            if (isMoving)
                Move();

            if (!Settings.GameSettings.GetIsDevMode())
                return;

            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (ScreensManager.HasActiveScreen())
                    return;

                isHidden = !isHidden;
                fpsDrawer.enabled = !isHidden;
                isMoving = true;
                ScreensManager.SetScreen(this);
            }
        }
        /// <summary>
        /// выключатель всех дебаггеров-частиц
        /// </summary>
        private void DisableDebuggers()
        {
            foreach (var d in Debuggers)
            {
                d.IsActive = false;
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
        /// Анимация движения
        /// </summary>
        private void Move()
        {
            // Установка точки, куда придёт окно
            Vector3 direction = Vector3.zero;
            direction.y = isHidden ? hiddenPositionY : showingPositionY;

            // Если анимация продолжается
            if (background.localPosition != direction)
            {
                background.localPosition = Vector3.MoveTowards(background.localPosition, direction, 100);
                return;
            }

            //Установка активности бэкграунда
            background.gameObject.SetActive(!isHidden);

            //Движение прекращено
            isMoving = false;

            //Активация текущего дебаггера
            activeDebugger.Activate();
        }

        /// <summary>
        /// Вызов при желании пользователя скрыть окно нажатием HideKey
        /// </summary>
        /// <returns></returns>
        public bool Hide()
        {
            isHidden = true;
            isMoving = true;

            return true;
        }

        /// <summary>
        /// Клавиша, нажатие которой скрывает окно
        /// </summary>
        /// <returns></returns>
        public KeyCode HideKey() => KeyCode.Escape;
    }
}