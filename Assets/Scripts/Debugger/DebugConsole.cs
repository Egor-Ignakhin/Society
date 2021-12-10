using System;
using System.Collections.Generic;
using System.Reflection;

using TMPro;

using UnityEngine;

namespace Society.Debugger
{
    /// <summary>
    /// Консоль разработчика
    /// </summary>
    public sealed class DebugConsole : MonoBehaviour, IDebug
    {
        [SerializeField] private GameObject FieldPrefab;// префаб поля в консоли
        [SerializeField] private Transform EventsContainer;// контейнер для полей
        [SerializeField] private TMP_InputField inputField;// поле ввода команд
        private bool commandWasChanged = false;// был ли изменен текст в поле ввода
        private string lastCommand;// последняя команда
        private readonly List<string> enteredCommands = new List<string>();// список введёных команд
        private int commandIterator = 0;// итератор для система управления стрелка вверх и вниз
        private bool isSelected;// выделено ли поле ввода

        public bool IsActive { get; set; } = true;
        GameObject IDebug.gameObject => gameObject;

        private void Awake()
        {
            CommandsContainer.SetDebugConsole(this);

            //Создание стандартных приветствий в консоли.
            Print("Hello, i'm Society Console.", true);
            Print("Use 'Help' to get page of help", true, 18);
        }
        private void Update()
        {
            if (!IsActive)
                return;
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))// отработка команды при нажатии энтера
            {
                if (!commandWasChanged || !isSelected)
                    return;

                ErrorsHandler(CommandExecution());
                commandWasChanged = false;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))// свайп вверх
            {
                WriteLastCommand();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))// свайп вниз
            {
                ClearInputField();
            }
        }
        /// <summary>
        /// вызывается для активации объекта
        /// </summary>
        public void Activate()
        {
            IsActive = true;
            gameObject.SetActive(true);
            inputField.ActivateInputField();
        }
        //считывание строки из поля ввода
        public void ReadCommand(string command)
        {
            if (!IsActive)
                return;
            if (command != string.Empty)// если строка не пустая
                lastCommand = command;
            commandWasChanged = true;// строка была изменена
        }
        /// <summary>
        /// создание нового поля команды
        /// </summary>
        /// <param name="value"></param>
        /// <param name="systemNote"></param>
        public void Print(string value, bool systemNote = false, int fontSize = 25)
        {
            var field = Instantiate(FieldPrefab, EventsContainer);
            var text = field.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            if (systemNote)// если команда системная (служебная)
            {
                text.color = Color.yellow;
            }
            else// если команда от пользователя
            {
                text.color = Color.cyan;
            }
            text.SetText(value);
            text.fontSize = fontSize;
        }
        /// <summary>
        /// вызовыватель исполнения команды
        /// </summary>
        /// <returns></returns>
        private string CommandExecution()
        {
            enteredCommands.Add(lastCommand);// добавление в лист команд 
            commandIterator = enteredCommands.Count;// присвоение итератору позиицю последней команды

            (string errorType, MethodInfo calledmthd, string command) = CommandsContainer.Execution(lastCommand);// вызов обработки команды и одновременно сбор возможной ошибки команды            

            //Выводим написанную пользователем команду
            Print("> " + lastCommand);
            if (errorType == string.Empty)// если команда прошла без ошибок
            {
                object[] l_args = new object[1] { command };
                calledmthd.Invoke(null, l_args);                
            }

            inputField.text = string.Empty;
            inputField.ActivateInputField();
            return errorType;
        }
        /// <summary>
        /// обработчик ошибок
        /// </summary>
        /// <param name="type"></param>
        private void ErrorsHandler(string type)
        {
            if (type == string.Empty)
                return;

            Print(type);
        }
        #region SelectDeselectEvents
        public void OnSelect()
        {
            isSelected = true;
        }
        public void OnDeselect()
        {
            isSelected = false;
        }
        #endregion
        private void WriteLastCommand()
        {
            if (commandIterator == 0)
                return;

            inputField.text = enteredCommands[--commandIterator];
            inputField.ActivateInputField();
        }
        /// <summary>
        /// очистка поля ввода
        /// </summary>
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