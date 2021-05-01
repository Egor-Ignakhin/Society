using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MenuScripts
{
    namespace PauseMenu
    {
        /// <summary>
        /// класс - управлящий меню паузой
        /// </summary>
        sealed class MenuPauseManager : MonoBehaviour
        {
            private MenuEventReceiver eventReceiver;// обработчик событий меню-паузы
            [SerializeField] private Transform mainParent;// контейнер сод. кнопки
            [SerializeField] GameObject MenuUI;// главный бэкграунд и носитель кнопок
            [SerializeField] private GameObject SettingsObj;// меню настроек            
            private FirstPersonController fps;// контроллёр игрока

            private void Start()
            {
                fps = FindObjectOfType<FirstPersonController>();
                eventReceiver = new MenuEventReceiver(MenuUI, mainParent, fps, SettingsObj);
            }
            private void Update()
            {
                eventReceiver.Update();
            }
            /// <summary>
            /// смена активности bloom'а
            /// </summary>
            /// <param name="t"></param>
            public void SetActiveGlobalBloom(Toggle t)
            {
                EffectsManager.Instance.SetEnableBloom(t.isOn);
            }
        }
        class MenuEventReceiver
        {
            private readonly GameObject menuUI;
            private readonly FirstPersonController fps;
            private readonly GameObject SettingsObj;
            private readonly CommandContainer commandContainer = new CommandContainer();
            private readonly AdvancedSettings advanced = new AdvancedSettings();
            private readonly Dictionary<GameObject, (Image image, TextMeshProUGUI text, int index)> btns = new Dictionary<GameObject, (Image, TextMeshProUGUI, int)>();
            public class AdvancedSettings
            {
                public Color SelectedColor { get; private set; } = new Color(0.33f, 0.33f, 0.33f, 1);// цвет при наведении на кнопку
                public Color DefaultColor { get; private set; } = new Color(0, 0, 0, 0);// обычный цвет кнопки
                public Color PressedColor { get; private set; } = new Color(0.25f, 0.25f, 0.25f, 1);// цвет при нажатии на кнопку
            }
            public MenuEventReceiver(GameObject menu, Transform mainParent, FirstPersonController fps, GameObject stn)
            {
                menuUI = menu;
                this.fps = fps;
                SettingsObj = stn;
                for (int i = 0; i < mainParent.childCount; i++)
                {
                    var c = mainParent.GetChild(i).GetChild(0).GetComponent<EventTrigger>();

                    EventTrigger.Entry entry = new EventTrigger.Entry
                    {
                        eventID = EventTriggerType.PointerEnter
                    };
                    entry.callback.AddListener((data) => { IntersectMouse(c.gameObject); });
                    c.triggers.Add(entry);

                    EventTrigger.Entry down = new EventTrigger.Entry
                    {
                        eventID = EventTriggerType.PointerDown
                    };
                    down.callback.AddListener((data) => { Down(c.gameObject); });
                    c.triggers.Add(down);

                    EventTrigger.Entry up = new EventTrigger.Entry
                    {
                        eventID = EventTriggerType.PointerUp
                    };
                    up.callback.AddListener((data) => { Up(c.gameObject); });
                    c.triggers.Add(up);

                    btns.Add(c.gameObject, (c.GetComponent<Image>(), c.transform.GetChild(0).GetComponent<TextMeshProUGUI>(), i));
                }
                DisableAllTriggers();
                commandContainer.SetEnableMenu(false, menuUI, fps);// выключение  меню
            }

            public void Update()
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    commandContainer.SetEnableMenu(!menuUI.activeInHierarchy, menuUI, fps);
                }
            }

            /// <summary>
            /// наведение на кнопку
            /// </summary>
            /// <param name="sender"></param>
            public void IntersectMouse(GameObject sender)
            {
                DisableAllTriggers();
                btns[sender].image.color = advanced.SelectedColor;
                btns[sender].text.color = Color.white;
            }
            /// <summary>
            /// нажатие на кнопку
            /// </summary>
            /// <param name="sender"></param>
            public void Down(GameObject sender)
            {
                btns[sender].image.color = advanced.PressedColor;
                btns[sender].text.color = Color.white;
                Doing(btns[sender].index);
            }
            //отжатие кнопки
            public void Up(GameObject sender)
            {
                DisableAllTriggers();
                btns[sender].image.color = advanced.SelectedColor;
                btns[sender].text.color = Color.white;
            }
            /// <summary>
            /// выключение видимости всех кнопок
            /// </summary>
            public void DisableAllTriggers()
            {
                foreach (var obj in btns)
                {
                    obj.Value.image.color = advanced.DefaultColor;
                    obj.Value.text.color = Color.black;
                }
            }
            private void Doing(int index)
            {
                switch (index)
                {
                    case 0:
                        commandContainer.FastLoad();
                        break;
                    case 1:
                        commandContainer.LoadLastCheckpoint();
                        break;
                    case 2:
                        commandContainer.NoteBook();
                        break;
                    case 3:
                        commandContainer.Hints();
                        break;
                    case 4:
                        commandContainer.PhotoMode();
                        break;
                    case 5:
                        commandContainer.Settings(SettingsObj);
                        break;
                    case 6:
                        commandContainer.ExitToMainMenu();
                        break;
                    case 7:
                        commandContainer.SetEnableMenu(false, menuUI, fps);
                        break;
                }
            }
        }
        public sealed class CommandContainer
        {
            public void FastLoad()
            {
                Debug.Log("TODO:FASTLOAD");
            }
            public void LoadLastCheckpoint()
            {
                Debug.Log("TODO:LOADLASTCHECKPOINT");
            }
            public void NoteBook()
            {
                Debug.Log("TODO:NOTEBOOK");
            }
            public void Hints()
            {
                Debug.Log("TODO:HINTS");
            }
            public void PhotoMode()
            {
                Debug.Log("TODO:PHOTOMODE");
            }
            public void Settings(GameObject SettingsObj)
            {
                SettingsObj.SetActive(!SettingsObj.activeInHierarchy);
            }
            public void ExitToMainMenu()
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesManager.MainMenu);
            }
            public void SetEnableMenu(bool v, GameObject menu, FirstPersonController fps)
            {
                menu.SetActive(v);
                // пауза при открытии инвентаря

                Cursor.visible = v;
                InventoryInput.Instance.DisableInventory();
                if (!v)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    fps.SetState(State.unlocked);
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    fps.SetState(State.locked);
                }
            }
        }

    }
}