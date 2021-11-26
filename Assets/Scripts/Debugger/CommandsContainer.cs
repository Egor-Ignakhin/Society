using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        private static readonly Dictionary<string, MethodInfo> consoleCommands;

        static CommandsContainer()
        {
            consoleCommands = new Dictionary<string, MethodInfo>();


            //Составляем список всех консольных команд
            var mthCommands = typeof(CommandsContainer)
                .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)//Собираем все методы класса
                .Where(x => x.GetCustomAttributes(typeof(ConsoleCommandAttribute), false).FirstOrDefault() != null);// Оставляем только имеющие атрибует ConsoleCommandAttribute

            //Проходим по списку и добавляем в словарь все команды.
            foreach (var method in mthCommands)
            {
                consoleCommands.Add(method.Name, typeof(CommandsContainer)
                    .GetMethod(method.Name, BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.Static));
            }
        }

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
        /// <returns></returns>.
        public static (string s, MethodInfo calledmthd, string outputedCommand) Execution(string input)
        {
            input = input.ToUpper();
            input = input.TrimStart(' ');
            input = input.TrimEnd(' ');

            (string type, string command) = Substring(input);
            if (!consoleCommands.ContainsKey(type))
                return ($"Erorr: command not found!", null, null);

            return (string.Empty, consoleCommands[type], command);
        }

        #region ConsoleCommands

        /// <summary>
        /// Вывод список всех возможных команд
        /// а также краткое описание работы каждой из них.
        /// </summary>
        /// <param name="_"></param>
        [ConsoleCommand("", "to get page of help")]
        private static void HELP(string _)
        {
            int k = 0;
            foreach (var cc in consoleCommands)
            {
                var ccAtr = cc.Value.GetCustomAttribute<ConsoleCommandAttribute>();

                //Если команда скрытая от обозрения
                if (ccAtr.IsHiddenCommand)
                    //Пропускаем
                    continue;

                debugConsole.Print($"{++k}. Use " +
                    $"'{cc.Key} {ccAtr.Args}' {ccAtr.Description}", true, 20);
            }                        
        }

        /// <summary>
        /// бесконесное здоровье
        /// </summary>
        /// <param name="c"></param>
        [ConsoleCommand("b", "to set the mode to 'Infinite Health'")]
        private static void ENDLESSHEALTH(string c)
        {
            int v = Convert.ToInt32(c);
            BasicNeeds.EndlessHealth = v == 1;// если 1 то беск. хп
        }

        /// <summary>
        /// бесконечная еда
        /// </summary>
        /// <param name="c"></param>
        [ConsoleCommand ("b", "to set the mode to 'Infinite Food'")]
        private static void ENDLESSFOOD(string c)
        {
            int v = Convert.ToInt32(c);
            BasicNeeds.EndlessFood = (v == 1);// если 1 то беск. хп
        }

        /// <summary>
        /// бесконечная вода
        /// </summary>
        /// <param name="c"></param>
        [ConsoleCommand ("b", "to set the mode to 'Infinite Water'")]
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
        [ConsoleCommand ("b", "to set the mode to 'Infinite Ammo'")]
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
        [ConsoleCommand("hrs:min", "to setup global time")]
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
        /// устанока здоровья
        /// </summary>
        /// <param name="command"></param>
        [ConsoleCommand("n","to set the amount of health to the player")]
        private static void SETHEALTH(string command)
        {
            int.TryParse(command, out int value);
            BasicNeeds.ForceSetHealth(value);
        }

        /// <summary>
        /// установка еды
        /// </summary>
        /// <param name="command"></param>
        [ConsoleCommand("n", "to set the amount of food to the player")]
        private static void SETFOOD(string command)
        {
            int.TryParse(command, out int value);
            BasicNeeds.ForceSetFood(value);
        }

        /// <summary>
        /// установка воды
        /// </summary>
        /// <param name="command"></param>
        [ConsoleCommand ("n", "to set the amount of water to the player")]
        private static void SETWATER(string command)
        {
            int.TryParse(command, out int value);
            BasicNeeds.ForceSetWater(value);
        }

        /// <summary>
        /// установка радиационного излучения
        /// </summary>
        /// <param name="command"></param>
        [ConsoleCommand("n", "to set the amount of radiation to the player")]
        private static void SETRADIATION(string command)
        {
            int.TryParse(command, out int value);
            BasicNeeds.ForceSetRadiation(value);
        }

        /// <summary>
        /// полное восстановление все необходимых для жизни стамин
        /// </summary>
        /// <param name="_"></param>
        [ConsoleCommand ("", "to fully restore the characteristics of the player")]
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
        [ConsoleCommand ("(x, y, z)", "to setup global postion of the player")]
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
        [ConsoleCommand ("", "to get info about health")]
        private static void GETHEALTH(string _) =>
            debugConsole.Print($"Health is" + BasicNeeds.Instance.Health);

        /// <summary>
        /// Пропуск активной задачи
        /// </summary>
        /// <param name="_"></param>
        [ConsoleCommand("", "to skip task")]
        private static void SKIPTASK(string _)
        {
            Missions.MissionsManager.Instance.SkipTask();

            debugConsole.Print($"Task skipped..");
        }

        #region Hidden commands

        /// <summary>
        /// В чём смысл жизни?
        /// </summary>
        /// <param name="_"></param>
        [ConsoleCommand(true)]
        private static void WHATISTHESENSEOFLIFE(string _)
        {
            debugConsole.Print("The meaning of life is human perfection.");
        }

        #endregion

        #endregion
    }
}