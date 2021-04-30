using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MenuScripts
{
    namespace PauseMenu
    {
        class MenuPauseManager : MonoBehaviour
        {
            private MenuEventReceiver eventReceiver;
            [SerializeField] private Transform mainParent;
            [SerializeField] GameObject MenuUI;
            [SerializeField] private GameObject SettingsObj;
            private bool MenuEnabled;
            private FirstPersonController fps;

            private void Start()
            {
                fps = FindObjectOfType<FirstPersonController>();
                eventReceiver = new MenuEventReceiver(MenuUI, mainParent, fps, SettingsObj);
                CommandContainer.SetEnableMenu(false, MenuUI, fps);
            }
            private void Update()
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CommandContainer.SetEnableMenu(MenuEnabled = !MenuEnabled, MenuUI, fps);
                }
            }
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
            public class AdvancedSettings
            {
                public Color SelectedColor { get; private set; } = new Color(0.33f, 0.33f, 0.33f, 1);
                public Color DefaultColor { get; private set; } = new Color(0, 0, 0, 0);
                public Color PressedColor { get; private set; } = new Color(0.25f, 0.25f, 0.25f, 1);
            }
            private readonly AdvancedSettings advanced = new AdvancedSettings();
            private readonly Dictionary<GameObject, (Image image, TextMeshProUGUI text, int index)> btns = new Dictionary<GameObject, (Image, TextMeshProUGUI, int)>();

            public MenuEventReceiver(GameObject menu, Transform mainParent, FirstPersonController fps, GameObject stn)
            {
                menuUI = menu;
                this.fps = fps;
                SettingsObj = stn;
                for (int i = 0; i < mainParent.childCount; i++)
                {
                    var c = mainParent.GetChild(i).GetChild(0).gameObject.GetComponent<EventTrigger>();

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
            }
            public void IntersectMouse(GameObject sender)
            {
                DisableAllTriggers();
                btns[sender].image.color = advanced.SelectedColor;
                btns[sender].text.color = Color.white;
            }
            public void Down(GameObject sender)
            {
                btns[sender].image.color = advanced.PressedColor;
                btns[sender].text.color = Color.white;
                Doing(btns[sender].index);
            }
            public void Up(GameObject sender)
            {
                DisableAllTriggers();
                btns[sender].image.color = advanced.SelectedColor;
                btns[sender].text.color = Color.white;
            }
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
                        CommandContainer.FastLoad();
                        break;
                    case 1:
                        CommandContainer.LoadLastCheckpoint();
                        break;
                    case 2:
                        CommandContainer.NoteBook();
                        break;
                    case 3:
                        CommandContainer.Hints();
                        break;
                    case 4:
                        CommandContainer.PhotoMode();
                        break;
                    case 5:
                        CommandContainer.Settings(SettingsObj);
                        break;
                    case 6:
                        CommandContainer.ExitToMainMenu();
                        break;
                    case 7:
                        CommandContainer.SetEnableMenu(false, menuUI, fps);
                        break;
                }
            }
        }
        class CommandContainer
        {

            public static void FastLoad()
            {
                Debug.Log("TODO:FASTLOAD");
            }
            public static void LoadLastCheckpoint()
            {
                Debug.Log("TODO:LOADLASTCHECKPOINT");
            }
            public static void NoteBook()
            {
                Debug.Log("TODO:NOTEBOOK");
            }
            public static void Hints()
            {
                Debug.Log("TODO:HINTS");
            }
            public static void PhotoMode()
            {
                Debug.Log("TODO:PHOTOMODE");
            }
            public static void Settings(GameObject SettingsObj)
            {
                SettingsObj.SetActive(!SettingsObj.activeInHierarchy);
            }
            public static void ExitToMainMenu()
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(ScenesManager.MainMenu);
            }
            public static void SetEnableMenu(bool v, GameObject menu, FirstPersonController fps)
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