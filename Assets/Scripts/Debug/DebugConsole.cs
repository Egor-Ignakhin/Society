using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Debugger
{
    sealed class DebugConsole : MonoBehaviour, IDebug
    {
        [SerializeField] private GameObject FieldPrefab;
        [SerializeField] private Transform EventsContainer;
        [SerializeField] private TMP_InputField inputField;
        private bool commandWasChanged = false;
        private string lastCommand;
        private readonly List<string> enteredCommands = new List<string>();
        private int commandIterator = 0;
        private bool isSelected;

        public bool Active { get; set; } = true;
        GameObject IDebug.gameObject => gameObject; 

        private void Awake()
        {
            CreateField("Society Console", true);
        }
        private void Update()
        {
            if (!Active)
                return;
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                if (!commandWasChanged || !isSelected)
                    return;

                ErrorsHandler(CommandExecution());
                commandWasChanged = false;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                WriteLastCommand();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ClearInputField();
            }
        }
        public void Activate()
        {
            Active = true;
            gameObject.SetActive(true);
            inputField.ActivateInputField();
        }
        public void ReadCommand(string command)
        {
            if (!Active)
                return;
            if (command != string.Empty)
                lastCommand = command;
            commandWasChanged = true;
        }
        private void CreateField(string value, bool systemNote = false)
        {
            var field = Instantiate(FieldPrefab, EventsContainer);
            var text = field.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (systemNote)
            {
                text.color = Color.yellow;
            }
            else
            {
                text.color = Color.cyan;
            }
            text.SetText(value);
        }
        private string CommandExecution()
        {
            enteredCommands.Add(lastCommand);
            commandIterator = enteredCommands.Count;

            string errorType = CommandsContainer.Execution(lastCommand);
            if (errorType == string.Empty)// if command was exe.
                CreateField("> " + lastCommand);

            inputField.text = string.Empty;
            inputField.ActivateInputField();
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
            if (commandIterator == 0)
                return;

            inputField.text = enteredCommands[--commandIterator];
            inputField.ActivateInputField();
        }
        private void ClearInputField()
        {
            if (commandIterator >= enteredCommands.Count - 1)
            {
                inputField.text = string.Empty;
                commandIterator = enteredCommands.Count;
            }
            else
            {
                inputField.text = enteredCommands[++commandIterator];
            }
            inputField.ActivateInputField();
        }       
    }
}