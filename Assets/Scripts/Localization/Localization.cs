using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Localization
{
    private static readonly Dictionary<int, string> dialogContents = new Dictionary<int, string>();// пути к диалогам    
    private static readonly Dictionary<int, string> taskContents = new Dictionary<int, string>();// пути к задачам
    private static Dictionary<string, string> upKeysDescriptions;// словаоь для подсказок (нажатия на клавишу)    
    internal static string GetUpKeyDescription(string mainType, KeyCode inputInteractive)
    {
        return $"{upKeysDescriptions[mainType]}({inputInteractive})";
    }
    private static HintsData hintsData;

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

        #region SetUpLeys
        upKeysDescriptions = new Dictionary<string, string>
        {
            {string.Empty, MainTypes.Default},
            {MainTypes.Item, MainTypes.Item}
        };
        #endregion
    }
    private static void InitHD()
    {
        string hdata = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Localization\\Descriptions.json");
        hintsData = JsonUtility.FromJson<HintsData>(hdata);
        hintsData.hints = new Dictionary<string, string>();
        for (int i = 0; i < hintsData.Types.Count; i++)
        {
            hintsData.hints.Add(hintsData.Types[i].Type, hintsData.Types[i].Desc);
        }
    }
    [System.Serializable]
    public class HintsData
    {
        public List<Hint> Types;
        public Dictionary<string, string> hints;
        public string GetHint(string t)
        {            
            return hints[t];
        }

        [System.Serializable]
        public class Hint
        {
            public string Type;
            public string Desc;
        }

    }
    public static string PathToCurrentLanguageContent(Type type, int missionNumber)// возвращает путь до нужного содержимого
    {
        switch (type)
        {
            case Type.Dialogs:
                return GetDialog(missionNumber);
            case Type.Tasks:
                return GetTask(missionNumber);
            default:
                return null;
        }
    }
    private static string GetDialog(int missionNumber) => dialogContents[missionNumber];

    private static string GetTask(int missionNumber) => taskContents[missionNumber];

    public static string GetHint(InteractiveObject interactiveObject)
    {
        if (hintsData == null)
            InitHD();
        return interactiveObject.Type != null ? hintsData.GetHint(interactiveObject.Type) : null;
    }
    public static string GetHint(int id)
    {
        if (hintsData == null)
            InitHD();
        return id != 0 ? hintsData.GetHint(((Inventory.ItemStates.ItemsID)id).ToString()) : null;
    }

}
