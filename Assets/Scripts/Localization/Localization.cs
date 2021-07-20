using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Localization
{    
    private static Dictionary<string, string> upKeysDescriptions;// словарь для подсказок (нажатия на клавишу)    
    internal static string GetUpKeyDescription(string mainType, KeyCode inputInteractive) =>
         $"{upKeysDescriptions[mainType]} ({inputInteractive})";

    private static HintsData hintsData;

    public enum Type { Dialogs, Tasks, Hints }
    public static class MainTypes
    {
        public const string Default = "Взаимодествовать";
        public const string Item = "Поднять предмет";
    }
    private static firstMTaskContent taskContent;
    private static firstMDialogsContent dialogContent;

    [System.Serializable]
    public class firstMTaskContent
    {
        public List<string> Tasks;// пути к задачам
        public string GetTask(int ch) => Tasks[ch];
    }

    [System.Serializable]
    public class firstMDialogsContent
    {
        public List<string> Dialogs;// пути к задачам
        public string GetTask(int ch) => Dialogs[ch];
    }

    public static void Init()
    {
        // инициализация путей
        #region SetDialogs
        {
            string data = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Localization\\Missions\\MissionDialogs_1.json");
            dialogContent = JsonUtility.FromJson<firstMDialogsContent>(data);
        }
        #endregion
        #region SetTasks
        {
            string data = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Localization\\Missions\\MissionTask_1.json");
            taskContent = JsonUtility.FromJson<firstMTaskContent>(data);
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
    public static string PathToCurrentLanguageContent(Type type, int missionNumber, int checkpoint)// возвращает путь до нужного содержимого
    {
        switch (type)
        {
            case Type.Dialogs:
                return GetDialog(missionNumber, checkpoint);
            case Type.Tasks:
                return GetTask(missionNumber, checkpoint);
            default:
                return null;
        }
    }
    private static string GetDialog(int missionNumber,int checkpoint) => dialogContent.GetTask(checkpoint);

    private static string GetTask(int missionNumber, int checkpoint) => taskContent.GetTask(checkpoint);

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
