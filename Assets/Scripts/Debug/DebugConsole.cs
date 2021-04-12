using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DebugConsole : MonoBehaviour
{
    [SerializeField] private GameObject FieldPrefab;
    [SerializeField] private Transform EventsContainer;
    [SerializeField] private TMP_InputField inputField;
    private bool commandWasChanged = false;
    private string lastCommand;
    private bool isSelected;
    //private string 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (!commandWasChanged || !isSelected)
                return;

            ErrorsHandler(CommandExecution());
            commandWasChanged = false;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            WriteLastCommand();
        }
    }
    public void ReadCommand(string command)
    {
        if(command != string.Empty)
            lastCommand = command;
        commandWasChanged = true;
    }
    private void CreateField(string value)
    {
        var field = Instantiate(FieldPrefab, EventsContainer);
        var text = field.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.SetText(value);
    }
    private string CommandExecution()
    {
        string errorType = CommandsContainer.Execution(lastCommand);
        if (errorType == string.Empty)// if command was exe.
            CreateField(lastCommand);

        inputField.text = string.Empty;
        return errorType;
    }
    private void ErrorsHandler(string type)
    {
        if (type == string.Empty)
            return;
        CreateField(type);
    }
    public void OnSelect()
    {
        isSelected = true;
    }
    public void OnDeselect()
    {
        isSelected = false;
    }
    private void WriteLastCommand()
    {
        inputField.text = lastCommand;
        inputField.Select();
    }
    public static class CommandsContainer
    {
        public delegate void Func(string command);
        public static Dictionary<string, Func> commands = new Dictionary<string, Func>();
        private static (string, string) Substring(string input)
        {
            string type = string.Empty;
            string command = string.Empty;
            try
            {
                type = input.Substring(0, input.IndexOf(' '));
                command = input.Substring(input.IndexOf(' ') + 1);
            }
            catch
            { }

            return (type, command);
        }
        public static string Execution(string input)
        {
            (string type, string command) = Substring(input);
            if (!commands.ContainsKey(type))
                return $"Erorr: {input}: command not found!";
            commands[type](command);
            return string.Empty;
        }
        static CommandsContainer()
        {
            commands.Add("SetTime", SetTime);
        }
        private static void SetTime(string command)
        {
            foreach (var c in command)
            {
                if (!char.IsDigit(c) && c != ':')
                {
                    return;
                }
            }
            try
            {
                Times.WorldTime.Instance.CurrentDate.ForceSetTime(command);
            }
            catch { }
        }
    }
}
