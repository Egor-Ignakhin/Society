using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Debugger
{
    public class CommandsContainer
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
                if (!input.Contains(' '))//if 1 word and no more
                {
                    type = input;
                }
                else// if 1 and more words
                {
                    type = input.Substring(0, input.IndexOf(' '));
                    command = input.Substring(input.IndexOf(' ') + 1);
                }
            }
            catch
            { }

            return (type, command);
        }
        public static string Execution(string input)
        {
            string output = "";
            for (int i = 0; i < input.Length; i++)
                output += char.ToUpper(input[i]);

            output = output.TrimStart(' ');
            output = output.TrimEnd(' ');

            (string type, string command) = Substring(output);
            if (!commands.ContainsKey(type))
                return $"Erorr: {input}: command not found!";
            if (gameMode == 0 && type != nameof(GAMEMODE))
                return $"Error: {input}: insufficient permissions!";
            commands[type](command);
            return string.Empty;
        }
        static CommandsContainer()
        {
            commands.Add(nameof(SETTIME), SETTIME);
            commands.Add(nameof(GAMEMODE), GAMEMODE);
            commands.Add(nameof(SETHEALTH), SETHEALTH);
            commands.Add(nameof(SETFOOD), SETFOOD);
            commands.Add(nameof(SETWATER), SETWATER);
            commands.Add(nameof(SETRADIATION), SETRADIATION);
            commands.Add(nameof(HEAL), HEAL);
        }
        private static void SETTIME(string command)
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
        private static void GAMEMODE(string command)
        {
            int.TryParse(command, out gameMode);
        }
        private static void SETHEALTH(string command)
        {
            int.TryParse(command, out int value);
            PlayerClasses.BasicNeeds.ForceSetHealth(value);
        }
        private static void SETFOOD(string command)
        {
            int.TryParse(command, out int value);
            PlayerClasses.BasicNeeds.ForceSetFood(value);
        }
        private static void SETWATER(string command)
        {
            int.TryParse(command, out int value);
            PlayerClasses.BasicNeeds.ForceSetWater(value);
        }
        private static void SETRADIATION(string command)
        {
            int.TryParse(command, out int value);
            PlayerClasses.BasicNeeds.ForceSetRadiation(value);
        }
        private static void HEAL(string _)
        {
            string input = "10000";
            SETHEALTH(input);
            SETFOOD(input);
            SETWATER(input);
            SETRADIATION("0");
        }
    }
}