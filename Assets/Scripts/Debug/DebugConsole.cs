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


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (!commandWasChanged || !isSelected)
                return;

            ErrorsHandler(CommandExecution());
            commandWasChanged = false;
        }
    }
    public void ReadCommand(string command)
    {
        lastCommand = command;
        commandWasChanged = true;
    }
    private string CommandExecution()
    {
        string errorType = string.Empty;
        var field = Instantiate(FieldPrefab, EventsContainer);
        var text = field.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.SetText(lastCommand);

        CommandsContainer.Execution(lastCommand);


        inputField.text = string.Empty;
        lastCommand = string.Empty;
        return errorType;
    }
    private void ErrorsHandler(string type)
    {
        if (type == string.Empty)
            return;
    }
    public void OnSelect()
    {
        isSelected = true;
    }
    public void OnDeselect()
    {
        isSelected = false;
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
        public static void Execution(string input)
        {
            (string type, string command) = Substring(input);
            if (!commands.ContainsKey(type))
                return;
            commands[type](command);
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
