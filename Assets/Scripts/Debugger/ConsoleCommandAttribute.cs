using System;

namespace Society.Debugger
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal sealed class ConsoleCommandAttribute : Attribute
    {
        /// <summary>
        /// �������� �������
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// ��������� �������
        /// </summary>
        public readonly string Args;

        /// <summary>
        /// ��� ������� �� ��������� �������?
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