namespace Society.Settings
{
    internal static class VideoSettings
    {
        private static GraphicsLevels grahicsLevel;
        private static ScreenResolutions screenResolution;
        private static bool isFullScreen;
        private static bool vSyncIsEnabled;
        private static AntiAliasingTypes antiAliasingType;

        #region PostProccess

        private static bool bloomIsEnabled;
        private static bool fogIsEnabled;

        #endregion

        internal static GraphicsLevels GetQualityLevel() => grahicsLevel;

        internal static void SetGraphicsQuality(GraphicsLevels value) => grahicsLevel = value;

        internal static bool GetIsFullScreen() => isFullScreen;

        internal static void SetIsFullScreen(bool value) => isFullScreen = value;

        internal static bool GetVSyncIsEnabled() => vSyncIsEnabled;

        internal static void SetVSyncIsEnabled(bool value) => vSyncIsEnabled = value;

        internal static ScreenResolutions GetScreenResolution() => screenResolution;

        internal static void SetScreenResolution(ScreenResolutions newResolution) => screenResolution = newResolution;

        internal static bool GetBloomIsEnabled() => bloomIsEnabled;

        internal static void SetBloomIsEnabled(bool value) => bloomIsEnabled = value;

        internal static bool GetFogIsEnabled() => fogIsEnabled;

        internal static void SetFogIsEnabled(bool value) => fogIsEnabled = value;
    }
}