using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Localization
{
    private static readonly Dictionary<int, string> dialogContents = new Dictionary<int, string>();// пути к диалогам    
    private static readonly Dictionary<int, string> taskContents = new Dictionary<int, string>();// пути к задачам

    private static readonly Dictionary<string, string> hintContents = new Dictionary<string, string>();// массив подсказок
    private static Dictionary<string, string> upKeysDescriptions;// словаоь для подсказок (нажатия на клавишу)

    internal static string GetUpKeyDescription(string mainType, KeyCode inputInteractive)
    {
        return $"{upKeysDescriptions[mainType]}({inputInteractive})";
    }

    public enum Type { Dialogs, Tasks, Hints }
    public static class MainTypes
    {
        public const string Default = "Взаимодествовать";
        public const string Item = "Поднять предмет ";
    }

    static Localization()
    {
        Init();
    }
    static void Init()
    {
        // инициализация путей
        #region SetDialogs
        {
            List<string> contents = new List<string>
            {
                Directory.GetCurrentDirectory() + "\\Localization\\Missions\\MissionDialogs_1.json",
                Directory.GetCurrentDirectory() + "\\Localization\\Missions\\MissionDialogs_2.json"
            };

            for (int i = 0; i < contents.Count; i++)
            {
                dialogContents.Add(i, contents[i]);
            }
        }
        #endregion
        #region SetTasks
        {
            List<string> contents = new List<string>
            {
                Directory.GetCurrentDirectory() + "\\Localization\\Missions\\MissionTask_1.json",
                Directory.GetCurrentDirectory() + "\\Localization\\Missions\\MissionTask_2.json"
            };

            for (int i = 0; i < contents.Count; i++)
            {
                taskContents.Add(i, contents[i]);
            }
        }
        #endregion
        #region SetHints
        string[] hints = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Localization\\Descriptions.json");
        char startSeparator = '[';
        char endSeparator = ']';
        for (int k = 0; k < hints.Length; k++)
        {
            string hint = hints[k];
            string key = "";
            bool wasChar = false;
            for (int i = 0; i < hint.Length; i++)
            {
                if (hint[i] == startSeparator)
                {
                    wasChar = true;
                    continue;
                }
                if (wasChar)
                {
                    if (hint[i] == endSeparator)
                        break;
                    key += hint[i];
                }
            }
            hint = hint.Replace(hint.Substring(0, hint.LastIndexOf(startSeparator) + 1), "");
            string value = hint.Replace(hint.Substring(hint.LastIndexOf(endSeparator)), "");
            hintContents.Add(key, value);
        }
        #endregion

        #region SetUpLeys
        upKeysDescriptions = new Dictionary<string, string>
        {
            {string.Empty, MainTypes.Default},
            {MainTypes.Item, MainTypes.Item}
        };
        #endregion
    }

    public static string PathToCurrentLanguageContent(Type type, int missionNumber)// возвращает путь до нужного содержимого
    {
        switch (type)
        {
            case Type.Dialogs:
                return GetDialog(missionNumber);
            case Type.Tasks:
                return GetTask(missionNumber);
            //case Type.Hints:
            //   return GetHint(interactiveObject);
            default:
                return null;
        }
    }
    private static string GetDialog(int missionNumber) => dialogContents[missionNumber];

    private static string GetTask(int missionNumber) => taskContents[missionNumber];

    public static string GetHint(InteractiveObject interactiveObject) => interactiveObject.Type != null ? hintContents[interactiveObject.Type] : null;
    public static string GetHint(int id) => id != 0 ? hintContents[((Inventory.ItemStates.ItemsID)id).ToString()] : null;

}
