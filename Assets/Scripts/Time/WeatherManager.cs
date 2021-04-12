using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public class Weather
    {
        public enum WeatherType { sunny, lowCloudy, highCloudy };
        public WeatherType currenType { get; private set; }
        public void SetWeather(WeatherType weatherType)
        {
            currenType = weatherType;
        }
        public void CreateСloudy()
        {

        }
    }
    public Weather CurrentWeather;
    private void Awake()
    {
        CurrentWeather = new Weather();
        CurrentWeather.SetWeather(Weather.WeatherType.lowCloudy);
    }

}
