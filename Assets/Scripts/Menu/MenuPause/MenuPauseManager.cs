using Inventory;
using MenuScripts.PauseMenu;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MenuScripts
{
    namespace PauseMenu
    {
        /// <summary>
        /// класс - управлящий меню паузой
        /// </summary>
        sealed class MenuPauseManager : MonoBehaviour, IGameScreen
        {
            private MenuEventReceiver menuEventReceiver;// обработчик событий меню-паузы
            [SerializeField] private Transform mainParent;// контейнер сод. кнопки
            [SerializeField] GameObject MenuUI;// главный бэкграунд и носитель кнопок
            [SerializeField] private GameObject SettingsObj;// меню настроек     
            [SerializeField] private Slider fovSlider;
            [SerializeField] private TextMeshProUGUI fovText;
            private FirstPersonController fpc;// контроллёр игрока
            private static CurrentGameSettings currentGameSettings;
            public static CurrentGameSettings GetCurrentGameSettings() => currentGameSettings;
            private readonly string pathForSettings = Directory.GetCurrentDirectory() + "\\Saves\\Settings.json";
            private EffectsManager effectsManager;
            private void Start()
            {
                fpc = FindObjectOfType<FirstPersonController>();
                effectsManager = FindObjectOfType<EffectsManager>();
                menuEventReceiver = new MenuEventReceiver(MenuUI, mainParent, fpc, SettingsObj, this, effectsManager);
                LoadData();

                fovSlider.value = (currentGameSettings.FOV - currentGameSettings.minFov) / (currentGameSettings.maxFov - currentGameSettings.minFov);

                fovText.SetText((Camera.main.fieldOfView = currentGameSettings.FOV).ToString());                
            }

            internal void Enable()
            {
                menuEventReceiver.Enable();
            }

            /// <summary>
            /// смена активности bloom'а
            /// </summary>
            /// <param name="t"></param>
            public void SetActiveGlobalBloom(Toggle t) => effectsManager.SetEnableBloom(t.isOn);

            private bool isInitialized;
            public void ChangeFovSlider()
            {
                if (!isInitialized)
                {
                    isInitialized = true;
                    return;
                }
                currentGameSettings.FOV = currentGameSettings.minFov + ((currentGameSettings.maxFov - currentGameSettings.minFov) * fovSlider.value);

                currentGameSettings.FOV = (float)System.Math.Round(currentGameSettings.FOV, 1);// округление до нормальных значений

                fovText.SetText((Camera.main.fieldOfView = currentGameSettings.FOV).ToString());
            }
            private void LoadData()
            {
                try
                {
                    string data = File.ReadAllText(pathForSettings);
                    currentGameSettings = JsonUtility.FromJson<CurrentGameSettings>(data);
                }
                catch
                {
                    currentGameSettings = new CurrentGameSettings();
                }
                if (currentGameSettings == null)
                    currentGameSettings = new CurrentGameSettings();
            }
            private void OnDisable() => SaveData();

            private void SaveData()
            {
                string data = JsonUtility.ToJson(currentGameSettings, true);
                File.WriteAllText(pathForSettings, data);
            }

            public void Hide() => menuEventReceiver.Disable();


            class MenuEventReceiver
            {
                private readonly GameObject menuUI;
                private readonly FirstPersonController fps;
                private readonly GameObject SettingsObj;
                private readonly CommandContainer commandContainer = new CommandContainer();
                private readonly AdvancedSettings advanced = new AdvancedSettings();
                private readonly Dictionary<GameObject, (Image image, TextMeshProUGUI text, int index)> btns = new Dictionary<GameObject, (Image, TextMeshProUGUI, int)>();
                private readonly MenuPauseManager menuPauseManager;
                private readonly EffectsManager effectsManager;
                public class AdvancedSettings
                {
                    public Color SelectedColor { get; private set; } = new Color(0.33f, 0.33f, 0.33f, 1);// цвет при наведении на кнопку
                    public Color DefaultColor { get; private set; } = new Color(0, 0, 0, 0);// обычный цвет кнопки
                    public Color PressedColor { get; private set; } = new Color(0.25f, 0.25f, 0.25f, 1);// цвет при нажатии на кнопку
                }
                public MenuEventReceiver(GameObject menu, Transform mainParent, FirstPersonController fps, GameObject stn, MenuPauseManager mpm, EffectsManager em)
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
                    menuPauseManager = mpm;
                    effectsManager = em;
                    Disable();
                }

                public void Enable() => commandContainer.SetEnableMenu(true, menuUI, fps, menuPauseManager, effectsManager);
                public void Disable() => commandContainer.SetEnableMenu(false, menuUI, fps, menuPauseManager, effectsManager);

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
                            commandContainer.SetEnableMenu(false, menuUI, fps, menuPauseManager, effectsManager);
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
                public void SetEnableMenu(bool v, GameObject menu, FirstPersonController fps, MenuPauseManager mpm, EffectsManager effectsManager)
                {
                    menu.SetActive(v);
                    // пауза при открытии инвентаря                                                        
                    if (!v)
                    {
                        ScreensManager.SetScreen(null);
                        fps.SetState(State.unlocked);
                    }
                    else
                    {
                        ScreensManager.SetScreen(mpm);
                        fps.SetState(State.locked);
                    }
                    effectsManager.SetEnableSimpleDOF(v);
                }
            }
        }
        [System.Serializable]
        public class CurrentGameSettings
        {
            public float minFov = 60;
            public float FOV = 70;
            public float maxFov = 80;
        }
    }
}
class GameSettings
{
    public static float MinFov() => MenuPauseManager.GetCurrentGameSettings().minFov;
    public static float FOV() => MenuPauseManager.GetCurrentGameSettings().FOV;
    public static float MaxFov() => MenuPauseManager.GetCurrentGameSettings().FOV;
}
