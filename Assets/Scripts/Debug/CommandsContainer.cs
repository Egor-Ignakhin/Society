using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger
{
    public static class CommandsContainer
    {
        public delegate void Func(string command);
        public static Dictionary<string, Func> commands = new Dictionary<string, Func>();
        private static int gameMode;// 0 is deadly; 1 is godly
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
            if (gameMode == 0 && type != nameof(GameMode))
                return $"Error: {input}: insufficient permissions!";
            commands[type](command);
            return string.Empty;
        }
        static CommandsContainer()
        {
            commands.Add(nameof(SetTime), SetTime);
            commands.Add(nameof(GameMode), GameMode);
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
        private static void GameMode(string command)
        {
            int.TryParse(command, out gameMode);
        }
    }
}