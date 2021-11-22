
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using UnityEngine;

namespace Society.Settings
{
    internal static class GameSettings
    {
        private static double musicVolume = 0.5D;
        private static double generalVolume = 1D;
        private static double fieldOfView = 70D;

        [JsonConverter(typeof(StringEnumConverter))]
        private static SystemLanguage systemLanguage = SystemLanguage.English;

        [JsonConverter(typeof(BoolToStringConverter))]
        private static bool isDevMode = false;

        internal static double GetMusicVolume() => musicVolume;

        internal static void SetMusicVolume(double value) => musicVolume = value;

        internal static double GetGeneralVolume() => generalVolume;

        internal static void SetGeneralVolume(double value) => generalVolume = value;

        internal static double GetFieldOfView() => fieldOfView;

        internal static void SetFieldOfView(double value) => fieldOfView = value;

        internal static bool GetIsDevMode() => isDevMode;

        internal static void SetIsDevMode(bool value) => isDevMode = value;
    }
}