using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Debugger
{
    public class CommandsContainer// класс содержащий список возможных команд для консоли и их исполняющий
    {
        public delegate void Func(string command);
        public static Dictionary<string, Func> commands;// словарь команд
        private static int gameMode;// 0 is deadly; 1 is godly
        /// <summary>
        /// // распил строки на тип команды и параметры для неё
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
        /// <summary>
        /// исполнитель команд команды
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
            commands = new Dictionary<string, Func> {
                {nameof(SETTIME), SETTIME},
                {nameof(GAMEMODE), GAMEMODE},
                {nameof(SETHEALTH), SETHEALTH},
                {nameof(SETFOOD), SETFOOD },
                {nameof(SETWATER), SETWATER },
                {nameof(SETRADIATION), SETRADIATION},
                {nameof(HEAL), HEAL}
            };
        }
        /// <summary>
        /// установка мирового времени
        /// </summary>
        /// <param name="command"></param>
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
        /// <summary>
        /// установка режимов : смертный, бог
        /// </summary>
        /// <param name="command"></param>
        private static void GAMEMODE(string command)
        {
            int.TryParse(command, out gameMode);
        }
        /// <summary>
        /// устанока здоровья
        /// </summary>
        /// <param name="command"></param>
        private static void SETHEALTH(string command)
        {
            int.TryParse(command, out int value);
            PlayerClasses.BasicNeeds.ForceSetHealth(value);
        }
        /// <summary>
        /// установка еды
        /// </summary>
        /// <param name="command"></param>
        private static void SETFOOD(string command)
        {
            int.TryParse(command, out int value);
            PlayerClasses.BasicNeeds.ForceSetFood(value);
        }
        /// <summary>
        /// установка воды
        /// </summary>
        /// <param name="command"></param>
        private static void SETWATER(string command)
        {
            int.TryParse(command, out int value);
            PlayerClasses.BasicNeeds.ForceSetWater(value);
        }
        /// <summary>
        /// установка радиационного излучения
        /// </summary>
        /// <param name="command"></param>
        private static void SETRADIATION(string command)
        {
            int.TryParse(command, out int value);
            PlayerClasses.BasicNeeds.ForceSetRadiation(value);
        }
        /// <summary>
        /// полное восстановление все необходимых для жизни стамин
        /// </summary>
        /// <param name="_"></param>
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