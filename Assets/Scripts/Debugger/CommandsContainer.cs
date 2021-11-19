using System;
using System.Collections.Generic;
using System.Linq;

using Society.Player;

namespace Society.Debugger
{
    /// <summary>
    /// Список возможных команд для консоли и их исполняющий
    /// </summary>
    public static class CommandsContainer
    {
        /// <summary>
        /// Консоль разработчика
        /// </summary>
        private static DebugConsole debugConsole;

        /// <summary>
        /// Cловарь команд
        /// </summary>
        public static IReadOnlyDictionary<string, Action<string>> commands = new Dictionary<string, Action<string>> {
                {nameof(SETTIME), SETTIME},
                {nameof(GAMEMODE), GAMEMODE},
                {nameof(SETHEALTH), SETHEALTH},
                {nameof(SETFOOD), SETFOOD},
                {nameof(SETWATER), SETWATER},
                {nameof(SETRADIATION), SETRADIATION},
                {nameof(HEAL), HEAL},
                {nameof(SETPOS), SETPOS},
                {nameof(ENDLESSHEALTH),ENDLESSHEALTH },
                {nameof(ENDLESSFOOD),ENDLESSFOOD },
                {nameof(ENDLESSWATER),ENDLESSWATER },
                {nameof(ENDLESSAMMO),ENDLESSAMMO },
                {nameof(HELP),HELP },
                {nameof(GET),GET },
                { nameof(SKIPTASK),SKIPTASK }
           };

        /// <summary>
        /// Тип игры, 0 = обычный, 1 = разработчик.
        /// </summary>
        private static int gameMode;

        public static void SetDebugConsole(DebugConsole dc) => debugConsole = dc;

        /// <summary>
        /// Распил строки на тип команды и параметры для неё
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
        /// Исполнитель команд
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Execution(string input)
        {
            input = input.ToUpper();
            input = input.TrimStart(' ');
            input = input.TrimEnd(' ');

            (string type, string command) = Substring(input);
            if (!commands.ContainsKey(type))
                return $"Erorr: {input}: command not found!";
#if !UNITY_EDITOR
            if ((gameMode == 0) && (type != nameof(GAMEMODE)) && (type != nameof(HELP)))
                return $"Error: {input}: insufficient permissions!";
#endif
            commands[type](command);
            if (type == nameof(HELP))
                return "HELP COMMAND";
            return string.Empty;
        }
        private static void HELP(string c)
        {
            int i = 0;
            debugConsole.Print($"{++i}. Use 'Settime hrs:min' to setup global time", true, 20);
            debugConsole.Print($"{++i}. Use 'GAMEMODE n' to setup god mode", true, 20);
            debugConsole.Print($"{++i}. Use 'SETHEALTH n' to set the amount of health to the player", true, 20);
            debugConsole.Print($"{++i}. Use 'SETFOOD n' to set the amount of food to the player", true, 20);
            debugConsole.Print($"{++i}. Use 'SETWATER n' to set the amount of water to the player", true, 20);
            debugConsole.Print($"{++i}. Use 'SETRADIATION n' to set the amount of radiation to the player", true, 20);
            debugConsole.Print($"{++i}. Use 'HEAL' to fully restore the characteristics of the player", true, 20);
            debugConsole.Print($"{++i}. Use 'SETPOS (x,y,z)' to setup global postion of the player", true, 20);
            debugConsole.Print($"{++i}. Use 'ENDLESSHEALTH n' to set the mode to 'Infinite Health'", true, 20);
            debugConsole.Print($"{++i}. Use 'ENDLESSFOOD n' to set the mode to 'Infinite Food'", true, 20);
            debugConsole.Print($"{++i}. Use 'ENDLESSWATER n' to set the mode to 'Infinite Water'", true, 20);
            debugConsole.Print($"{++i}. Use 'ENDLESSAMMO n' to set the mode to 'Infinite Ammo'", true, 20);
            debugConsole.Print($"{++i}. Use 'HELP' to get help", true, 20);
            debugConsole.Print($"{++i}. Use 'GET HEALTH' to get info about health ", true, 20);
            debugConsole.Print($"{++i}. Use 'SKIPTASK _ to skip task", true, 20);
        }

        /// <summary>
        /// бесконесное здоровье
        /// </summary>
        /// <param name="c"></param>
        private static void ENDLESSHEALTH(string c)
        {
            int v = Convert.ToInt32(c);
            BasicNeeds.EndlessHealth = (v == 1);// если 1 то беск. хп
        }

        /// <summary>
        /// бесконечная еда
        /// </summary>
        /// <param name="c"></param>
        private static void ENDLESSFOOD(string c)
        {
            int v = Convert.ToInt32(c);
            BasicNeeds.EndlessFood = (v == 1);// если 1 то беск. хп
        }

        /// <summary>
        /// бесконечная вода
        /// </summary>
        /// <param name="c"></param>
        private static void ENDLESSWATER(string c)
        {
            int v = Convert.ToInt32(c);
            BasicNeeds.EndlessWater = (v == 1);// если 1 то беск. хп
        }

        /// <summary>
        /// Запрещает патронам тратиться.
        /// 0 = обычный. 1 = бесконечный.
        /// </summary>
        /// <param name="c"></param>
        private static void ENDLESSAMMO(string c)
        {
            if (int.TryParse(c, out int v))
            {
                Shoot.Gun.EndlessBullets = v == 1;
            }
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
                Times.WorldTime.CurrentDate.ForceSetTime(command);
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
            BasicNeeds.ForceSetHealth(value);
        }

        /// <summary>
        /// установка еды
        /// </summary>
        /// <param name="command"></param>
        private static void SETFOOD(string command)
        {
            int.TryParse(command, out int value);
            BasicNeeds.ForceSetFood(value);
        }

        /// <summary>
        /// установка воды
        /// </summary>
        /// <param name="command"></param>
        private static void SETWATER(string command)
        {
            int.TryParse(command, out int value);
            BasicNeeds.ForceSetWater(value);
        }

        /// <summary>
        /// установка радиационного излучения
        /// </summary>
        /// <param name="command"></param>
        private static void SETRADIATION(string command)
        {
            int.TryParse(command, out int value);
            BasicNeeds.ForceSetRadiation(value);
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

        /// <summary>
        /// Установка координат игрока
        /// </summary>
        /// <param name="pos"></param>
        private static void SETPOS(string posStr)// (50,60,70.1)
        {
            posStr = System.Text.RegularExpressions.Regex.Replace(posStr, @"[()]", "");

            var ss = posStr.Split(',');
            int.TryParse(ss[0], out int x);
            int.TryParse(ss[1], out int y);
            int.TryParse(ss[2], out int z);
            UnityEngine.Vector3 pos = new UnityEngine.Vector3(x, y, z);

            BasicNeeds.Instance.transform.position = pos;
        }

        /// <summary>
        /// Возвращает значение свойства
        /// </summary>
        /// <param name="args"></param>
        private static void GET(string args)
        {
            if (args == "HEALTH")
            {
                debugConsole.Print($"Health is" + BasicNeeds.Instance.Health);
            }
        }

        /// <summary>
        /// Пропуск активной задачи
        /// </summary>
        /// <param name="_"></param>
        private static void SKIPTASK(string _)
        {
            Missions.MissionsManager.Instance.SkipTask();
        }
    }
}