using System;

namespace Society.Debugger
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal sealed class ConsoleCommandAttribute : Attribute
    {
        /// <summary>
        /// Описание команды
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// Аргументы команды
        /// </summary>
        public readonly string Args;

        /// <summary>
        /// Это скрытая от обозрения команда?
        /// </summary>
        public readonly bool IsHiddenCommand;

        public ConsoleCommandAttribute(string args = "", string description = "", bool isHiddenCommand = false)
        {
            Args = args;
            Description = description;
            IsHiddenCommand = isHiddenCommand;
        }
        public ConsoleCommandAttribute(bool isHiddenCommand)
        {
            IsHiddenCommand = isHiddenCommand;
        }
    }
}