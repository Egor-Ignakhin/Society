using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using UnityEngine;

namespace Society.Settings
{
    internal static class GameSettings
    {
        private static double musicVolume = 0.5D;
        private static double generalVolume;
        private static double fieldOfView;

        [JsonConverter(typeof(StringEnumConverter))]
        private static SystemLanguage systemLanguage;

        private static bool isDevMode;        

        internal static double GetMusicVolume() => musicVolume;

        internal static void SetMusicVolume(double value) => musicVolume = value;
    }
}