using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using UnityEngine;

using static UnityEngine.Rendering.HighDefinition.HDAdditionalCameraData;

namespace Society.Settings
{
    internal static class GameSettings
    {
        private static SerializableGameSettins serializableGameSettins;

        [System.Serializable]
        public class SerializableGameSettins
        {
            private double musicVolume = 0.5D;
            private double generalVolume = 1D;
            private double fieldOfView = 70D;

            [JsonConverter(typeof(StringEnumConverter))]
            private SystemLanguage systemLanguage = SystemLanguage.English;

            [JsonConverter(typeof(BoolToStringConverter))]
            private bool isDevMode = false;

            private double mouseSensivity = 3D;
            private KeyCode moveFrontKeyCode = KeyCode.W;
            private KeyCode moveBackKeyCode = KeyCode.S;
            private KeyCode moveLeftKeyCode = KeyCode.A;
            private KeyCode moveRightKeyCode = KeyCode.D;
            private KeyCode leanLeftKeyCode = KeyCode.Q;
            private KeyCode leanRightKeyCode = KeyCode.E;
            private KeyCode jumpKeyCode = KeyCode.Space;
            private KeyCode crouchKeyCode = KeyCode.LeftControl;
            private KeyCode proneKeyCode = KeyCode.Z;
            private KeyCode sprintKeyCode = KeyCode.LeftShift;
            private KeyCode inventoryKeyCode = KeyCode.Tab;
            private KeyCode interactionKeyCode = KeyCode.F;
            private KeyCode reloadKeyCode = KeyCode.R;

            private GraphicsLevels grahicsLevel;
            private ScreenResolutions screenResolution;
            private bool isFullScreen;
            private bool vSyncIsEnabled;
            private AntialiasingMode antiAliasingMode;

            #region PostProccess

            private bool bloomIsEnabled;
            private bool fogIsEnabled;

            #endregion
        }

        public static SerializableGameSettins GetSerializableSettins() => serializableGameSettins;
        public static void SetSerializableSettins(SerializableGameSettins value) => serializableGameSettins = value;

        #region Game

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

        #endregion

        #region Input

        private static double mouseSensivity = 3D;
        private static KeyCode moveFrontKeyCode = KeyCode.W;
        private static KeyCode moveBackKeyCode = KeyCode.S;
        private static KeyCode moveLeftKeyCode = KeyCode.A;
        private static KeyCode moveRightKeyCode = KeyCode.D;
        private static KeyCode leanLeftKeyCode = KeyCode.Q;
        private static KeyCode leanRightKeyCode = KeyCode.E;
        private static KeyCode jumpKeyCode = KeyCode.Space;
        private static KeyCode crouchKeyCode = KeyCode.LeftControl;
        private static KeyCode proneKeyCode = KeyCode.Z;
        private static KeyCode sprintKeyCode = KeyCode.LeftShift;
        private static KeyCode inventoryKeyCode = KeyCode.Tab;
        private static KeyCode interactionKeyCode = KeyCode.F;
        private static KeyCode reloadKeyCode = KeyCode.R;

        internal static double GetMouseSensivity() => mouseSensivity;
        internal static void SetMouseSensivity(double value) => mouseSensivity = value;

        internal static KeyCode GetMoveFrontKeyCode() => moveFrontKeyCode;
        internal static KeyCode SetMoveFrontKeyCode(KeyCode value) => moveFrontKeyCode = value;

        internal static KeyCode GetMoveBackKeyCode() => moveBackKeyCode;
        internal static KeyCode SetMoveBackKeyCode(KeyCode value) => moveBackKeyCode = value;

        internal static KeyCode GetMoveLeftKeyCode() => moveLeftKeyCode;
        internal static KeyCode SetMoveLeftKeyCode(KeyCode value) => leanLeftKeyCode = value;

        internal static KeyCode GetMoveRightKeyCode() => moveRightKeyCode;
        internal static KeyCode SetMoveRightKeyCode(KeyCode value) => moveRightKeyCode = value;

        internal static KeyCode GetLeanLeftKeyCode() => leanLeftKeyCode;
        internal static KeyCode SetLeanLeftKeyCode(KeyCode value) => leanLeftKeyCode = value;

        internal static KeyCode GetLeanRightKeyCode() => leanRightKeyCode;
        internal static KeyCode SetLeanRightKeyCode(KeyCode value) => leanRightKeyCode = value;

        internal static KeyCode GetJumpKeyCode() => jumpKeyCode;
        internal static KeyCode SetJumpKeyCode(KeyCode value) => jumpKeyCode = value;

        internal static KeyCode GetCrouchKeyCode() => crouchKeyCode;
        internal static KeyCode SetCrouchKeyCode(KeyCode value) => crouchKeyCode = value;

        internal static KeyCode GetProneKeyCode() => proneKeyCode;
        internal static KeyCode SetProneKeyCode(KeyCode value) => proneKeyCode = value;

        internal static KeyCode GetSprintKeyCode() => sprintKeyCode;
        internal static KeyCode SetSprintKeyCode(KeyCode value) => sprintKeyCode = value;

        internal static KeyCode GetInventoryKeyCode() => inventoryKeyCode;
        internal static KeyCode SeInventoryKeyCode(KeyCode value) => inventoryKeyCode = value;

        internal static KeyCode GetInteractionKeyCode() => interactionKeyCode;
        internal static KeyCode SetInteractionKeyCode(KeyCode value) => interactionKeyCode = value;

        internal static KeyCode GetReloadKeyCode() => reloadKeyCode;
        internal static KeyCode SetReloadKeyCode(KeyCode value) => reloadKeyCode = value;

        #endregion

        #region Video

        private static GraphicsLevels grahicsLevel;
        private static ScreenResolutions screenResolution;
        private static bool isFullScreen;
        private static bool vSyncIsEnabled;
        private static AntialiasingMode antiAliasingMode;

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

        internal static void SetScreenResolution(ScreenResolutions value) => screenResolution = value;

        internal static AntialiasingMode GetAntialiasingType() => antiAliasingMode;

        internal static void SetAntialiasingType(AntialiasingMode value) => antiAliasingMode = value;


        #region PostProccess

        internal static bool GetBloomIsEnabled() => bloomIsEnabled;

        internal static void SetBloomIsEnabled(bool value) => bloomIsEnabled = value;

        internal static bool GetFogIsEnabled() => fogIsEnabled;

        internal static void SetFogIsEnabled(bool value) => fogIsEnabled = value;

        #endregion

        #endregion
    }
}