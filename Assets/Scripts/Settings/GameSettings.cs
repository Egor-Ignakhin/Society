using System;

using UnityEngine;

namespace Society.Settings
{
    internal static class GameSettings
    {
        private static float musicVolume = 0.5F;
        private static float generalVolume;
        private static float fieldOfView;
        private static SystemLanguage systemLanguage;
        private static bool isDevMode;        

        internal static float GetMusicVolume() => musicVolume;

        internal static void SetMusicVolume(float value) => musicVolume = value;
    }
}