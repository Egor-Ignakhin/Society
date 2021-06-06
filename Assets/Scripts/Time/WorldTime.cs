using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
namespace Times
{
    public sealed class WorldTime : MonoBehaviour
    {
        private static int Time;// добавленное время за такт
        private int additionalTime = 1;// множитель времени
        private static Date currentDate;// текущая дата
        public static Date CurrentDate
        {
            get
            {
                if (currentDate == null)
                {
                    LoadDate();
                }
                return currentDate;
            }
            private set
            {
                currentDate = value;
            }
        }

        private float waitTime = 1;// ожидание следующего такта
        public static string dateFolder = Directory.GetCurrentDirectory() + "\\Saves";// папка с датой
        public static string dateFile = "\\Date.json";// дата

        public delegate void ChangeTime(string time);
        public event ChangeTime ChangeTimeEvent;// событие изменения времени

        public delegate void ChangeTimeInNumbers(int s, int m, int h);
        public event ChangeTimeInNumbers ChangeTimeEventInNumbers;// событие изменения времени

        private void OnEnable()
        {
            LoadData();
            StartCoroutine(nameof(TimeThread));
        }

        /// <summary>
        /// сохранение данных
        /// </summary>
        private void SaveData()
        {
            // Start save date
            string savingDate = JsonUtility.ToJson(CurrentDate, true);
            File.WriteAllText(dateFolder + dateFile, savingDate);
            // End save date
        }
        private void Start()
        {
            InvokeTimeEvent();
        }

        /// <summary>
        /// ускорение времени
        /// </summary>
        /// <param name="timeMultiply"></param>
        internal void IncreaseSpeed(float timeMultiply)
        {
            waitTime /= timeMultiply;
        }
        /// <summary>
        /// замедление времени
        /// </summary>
        /// <param name="timeMultiply"></param>
        internal void ReduceSpeed(float timeMultiply)
        {
            waitTime *= timeMultiply;
        }
        #region loadDate
        /// <summary>
        /// загрузка даты
        /// </summary>
        private static void LoadDate()
        {
            try
            {
                string data = File.ReadAllText(dateFolder + dateFile);
                currentDate = JsonUtility.FromJson<Date>(data);
            }
            catch
            {
                currentDate = new Date();
                if (!Directory.Exists(dateFolder))
                {
                    Directory.CreateDirectory(dateFolder);
                    File.Create(dateFolder + dateFile);
                }
            }
        }
        #endregion
        private void LoadData()
        {
            // Start load date
            if (CurrentDate == null)
                LoadDate();
            // End load Date
        }
        /// <summary>
        /// поток увеличивающий время
        /// </summary>
        /// <returns></returns>
        private IEnumerator TimeThread()
        {
            while (true)
            {
                Time += additionalTime;

                CurrentDate.seconds += Time;
                CurrentDate.SetTime();

                Time = 0;
                InvokeTimeEvent();

                yield return new WaitForSeconds(waitTime);
            }
        }
        private void InvokeTimeEvent()// вызов события изменения времени с передачей аргументом часов и минут
        {
            string h = CurrentDate.hours.ToString(); h = h.Length > 1 ? h : '0' + h;
            string m = CurrentDate.minutes.ToString(); m = m.Length > 1 ? m : '0' + m;
            ChangeTimeEvent?.Invoke(h + ':' + m);

            ChangeTimeEventInNumbers?.Invoke(CurrentDate.seconds, CurrentDate.minutes, CurrentDate.hours);
        }
        private void OnDisable()
        {
            StopCoroutine(nameof(TimeThread));
            SaveData();
        }
    }
    [Serializable]
    public class Date
    {
        public int seconds;
        public int minutes;
        public int hours;
        public int days;

        public void SetTime()
        {
            if (seconds >= 60)
            {
                minutes++;
                seconds -= 60;
                SetTime();
            }
            if (minutes >= 60)
            {
                hours++;
                minutes -= 60;
                SetTime();
            }
            if (hours >= 24)
            {
                days++;
                hours -= 24;
                SetTime();
            }
        }
        public async void ForceSetTime(string hoursMins)
        {
            await Task.Delay(10);//delay for possible constructors
            try
            {
                int.TryParse(hoursMins.Substring(0, hoursMins.Contains(":") ?
                    hoursMins.IndexOf(':') : hoursMins.Length), out hours);
                int.TryParse(hoursMins.Substring(hoursMins.IndexOf(':') + 1), out minutes);
                seconds = 0;
            }
            catch { }
        }
    }
}