using System.Collections.Generic;
using System.IO;

public static class Localization
{
    private static List<string> dialogContents = new List<string>();// пути к диалогам
    private static List<string> taskContents = new List<string>();// пути к задачам

    private static Dictionary<string, string> hintContents = new Dictionary<string, string>();// массив подсказок
    public enum Type { Dialogs, Tasks, Hints }

    static Localization()
    {
        // инициализация путей
        #region SetDialogs
        {
            string firstMissionContent = Directory.GetCurrentDirectory() + "\\Localization\\MissionDialogs_1.json";
            dialogContents.Add(firstMissionContent);
        }
        #endregion
        #region SetTasks
        {
            string firstTaskContent = Directory.GetCurrentDirectory() + "\\Localization\\MissionTask_1.json";
            taskContents.Add(firstTaskContent);
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
            //    return GetHint(interactiveObject);
            default:
                return null;
        }
    }    
    private static string GetDialog(int missionNumber)
    {
        return dialogContents[missionNumber];
    }
    private static string GetTask(int missionNumber)
    {
        return taskContents[missionNumber];
    }
    public static string GetHint(InteractiveObject interactiveObject)
    {
        try
        {
            return hintContents[interactiveObject.GetTypeObject()];
        }
        catch
        {
            return null;
        }
    }
}
