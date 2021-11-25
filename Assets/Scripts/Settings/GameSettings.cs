using System.Collections.Generic;
using System.IO;

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
            public double musicVolume = 0.5D;
            public double generalVolume = 1D;
            public double fieldOfView = 70D;

            [JsonConverter(typeof(StringEnumConverter))]
            public SystemLanguage systemLanguage = SystemLanguage.English;

            [JsonConverter(typeof(BoolToStringConverter))]
            public bool isDevMode = false;

            public double mouseSensivity = 3D;
            public KeyCode moveFrontKeyCode = KeyCode.W;
            public KeyCode moveBackKeyCode = KeyCode.S;
            public KeyCode moveLeftKeyCode = KeyCode.A;
            public KeyCode moveRightKeyCode = KeyCode.D;
            public KeyCode leanLeftKeyCode = KeyCode.Q;
            public KeyCode leanRightKeyCode = KeyCode.E;
            public KeyCode jumpKeyCode = KeyCode.Space;
            public KeyCode crouchKeyCode = KeyCode.LeftControl;
            public KeyCode proneKeyCode = KeyCode.Z;
            public KeyCode sprintKeyCode = KeyCode.LeftShift;
            public KeyCode inventoryKeyCode = KeyCode.Tab;
            public KeyCode interactionKeyCode = KeyCode.F;
            public KeyCode reloadKeyCode = KeyCode.R;

            public GraphicsLevels grahicsLevel;
            public ScreenResolutions screenResolution;
            public bool isFullScreen;
            public bool vSyncIsEnabled;
            public AntialiasingMode antiAliasingMode;

            #region PostProccess

            public bool bloomIsEnabled;
            public bool fogIsEnabled;



            #endregion
        }
        public static string GetPathToSettings() => Directory.GetCurrentDirectory() + "\\Saves\\Settings.json";
        static GameSettings()
        {
            var data = File.ReadAllText(GetPathToSettings());

            SetSerializableSettins(JsonConvert.DeserializeObject<SerializableGameSettins>(data));
        }

        public static SerializableGameSettins GetSerializableSettins() => serializableGameSettins;
        public static void SetSerializableSettins(SerializableGameSettins value) => serializableGameSettins = value;

        #region Game

        internal static double GetMusicVolume() => serializableGameSettins.musicVolume;

        internal static void SetMusicVolume(double value) => serializableGameSettins.musicVolume = value;

        internal static double GetGeneralVolume() => serializableGameSettins.generalVolume;

        internal static void SetGeneralVolume(double value) => serializableGameSettins.generalVolume = value;

        internal static double GetFieldOfView() => serializableGameSettins.fieldOfView;

        internal static void SetFieldOfView(double value) => serializableGameSettins.fieldOfView = value;

        internal static SystemLanguage GetSystemLanguage() => serializableGameSettins.systemLanguage;
        internal static void SetLanguage(SystemLanguage systemLanguage)=>serializableGameSettins.systemLanguage = systemLanguage;        

        internal static bool GetIsDevMode() => serializableGameSettins.isDevMode;

        internal static void SetIsDevMode(bool value) => serializableGameSettins.isDevMode = value;

        #endregion

        #region Input

        internal static double GetMouseSensivity() => serializableGameSettins.mouseSensivity;
        internal static void SetMouseSensivity(double value) => serializableGameSettins.mouseSensivity = value;

        internal static KeyCode GetMoveFrontKeyCode() => serializableGameSettins.moveFrontKeyCode;
        internal static void SetMoveFrontKeyCode(KeyCode value) => serializableGameSettins.moveFrontKeyCode = value;

        internal static KeyCode GetMoveBackKeyCode() => serializableGameSettins.moveBackKeyCode;
        internal static void SetMoveBackKeyCode(KeyCode value) => serializableGameSettins.moveBackKeyCode = value;

        internal static KeyCode GetMoveLeftKeyCode() => serializableGameSettins.moveLeftKeyCode;
        internal static void SetMoveLeftKeyCode(KeyCode value) => serializableGameSettins.leanLeftKeyCode = value;

        internal static KeyCode GetMoveRightKeyCode() => serializableGameSettins.moveRightKeyCode;
        internal static void SetMoveRightKeyCode(KeyCode value) => serializableGameSettins.moveRightKeyCode = value;

        internal static KeyCode GetLeanLeftKeyCode() => serializableGameSettins.leanLeftKeyCode;
        internal static void SetLeanLeftKeyCode(KeyCode value) => serializableGameSettins.leanLeftKeyCode = value;

        internal static KeyCode GetLeanRightKeyCode() => serializableGameSettins.leanRightKeyCode;
        internal static void SetLeanRightKeyCode(KeyCode value) => serializableGameSettins.leanRightKeyCode = value;

        internal static KeyCode GetJumpKeyCode() => serializableGameSettins.jumpKeyCode;
        internal static void SetJumpKeyCode(KeyCode value) => serializableGameSettins.jumpKeyCode = value;

        internal static KeyCode GetCrouchKeyCode() => serializableGameSettins.crouchKeyCode;
        internal static void SetCrouchKeyCode(KeyCode value) => serializableGameSettins.crouchKeyCode = value;

        internal static KeyCode GetProneKeyCode() => serializableGameSettins.proneKeyCode;
        internal static void SetProneKeyCode(KeyCode value) => serializableGameSettins.proneKeyCode = value;

        internal static KeyCode GetSprintKeyCode() => serializableGameSettins.sprintKeyCode;
        internal static void SetSprintKeyCode(KeyCode value) => serializableGameSettins.sprintKeyCode = value;

        internal static KeyCode GetInventoryKeyCode() => serializableGameSettins.inventoryKeyCode;
        internal static void SeInventoryKeyCode(KeyCode value) => serializableGameSettins.inventoryKeyCode = value;

        internal static KeyCode GetInteractionKeyCode() => serializableGameSettins.interactionKeyCode;
        internal static void SetInteractionKeyCode(KeyCode value) => serializableGameSettins.interactionKeyCode = value;

        internal static KeyCode GetReloadKeyCode() => serializableGameSettins.reloadKeyCode;
        internal static void SetReloadKeyCode(KeyCode value) => serializableGameSettins.reloadKeyCode = value;

        #endregion

        #region Video

        

        private static Dictionary<ScreenResolutions, (int widht, int height)> resolutionsDictionaty
            = new Dictionary<ScreenResolutions, (int widht, int height)>
        {
                { ScreenResolutions._1920x1080, (1920, 1080) },
                { ScreenResolutions._1680x1050, (1680, 1050) },
                { ScreenResolutions._1600x1024, (1600, 1024) },
                { ScreenResolutions._1600x900, (1600, 900)   },
                { ScreenResolutions._1440x900, (1440, 900)   },
                { ScreenResolutions._1366x768, (1366, 768)   },
                { ScreenResolutions._1360x768, (1360, 768)   },
                { ScreenResolutions._1280x1024, (1280, 1024) },
                { ScreenResolutions._1280x960, (1280, 960)   },
                { ScreenResolutions._1280x800, (1280, 800)   },
                { ScreenResolutions._1280x768, (1280, 768)   },
                { ScreenResolutions._1280x720, (1280, 720)   },
                { ScreenResolutions._1176x664, (1176, 664)   },
                { ScreenResolutions._1152x864, (1150, 864)   },
                { ScreenResolutions._1024x768, (1024, 768)   },
                { ScreenResolutions._800x600, (800, 600)     },
                { ScreenResolutions._720x576, (720, 576)     },
                { ScreenResolutions._720x480, (720, 480)     },
                { ScreenResolutions._640x480, (640, 480)     }
        };

        
        internal static GraphicsLevels GetQualityLevel() => serializableGameSettins.grahicsLevel;

        internal static void SetGraphicsQuality(GraphicsLevels value) => serializableGameSettins.grahicsLevel = value;

        internal static bool GetIsFullScreen() => serializableGameSettins.isFullScreen;

        internal static void SetIsFullScreen(bool value) => serializableGameSettins.isFullScreen = value;

        internal static bool GetVSyncIsEnabled() => serializableGameSettins.vSyncIsEnabled;

        internal static void SetVSyncIsEnabled(bool value) => serializableGameSettins.vSyncIsEnabled = value;

        internal static (int width, int height) GetAndDescriptScreenResolution()
        {
            return resolutionsDictionaty[serializableGameSettins.screenResolution];
        }

        internal static ScreenResolutions GetScreenResolution() => serializableGameSettins.screenResolution;

        internal static void SetScreenResolution(ScreenResolutions value) => serializableGameSettins.screenResolution = value;

        internal static AntialiasingMode GetAntialiasingType() => serializableGameSettins.antiAliasingMode;

        internal static void SetAntialiasingType(AntialiasingMode value) => serializableGameSettins.antiAliasingMode = value;


        #region PostProccess

        internal static bool GetBloomIsEnabled() => serializableGameSettins.bloomIsEnabled;

        internal static void SetBloomIsEnabled(bool value) => serializableGameSettins.bloomIsEnabled = value;

        internal static bool GetFogIsEnabled() => serializableGameSettins.fogIsEnabled;

        internal static void SetFogIsEnabled(bool value) => serializableGameSettins.fogIsEnabled = value;

        #endregion

        #endregion
    }
}