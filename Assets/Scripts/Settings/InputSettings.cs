using System;

using UnityEngine;

namespace Society.Settings
{
    internal static class InputSettings
    {
        private static double mouseSensivity = 3D;
        private static KeyCode moveFrontKeyCode;
        private static KeyCode moveBackKeyCode;
        private static KeyCode moveLeftKeyCode;
        private static KeyCode moveRightKeyCode;
        private static KeyCode leanLeftKeyCode;
        private static KeyCode leanRightKeyCode;
        private static KeyCode jumpKeyCode;
        private static KeyCode crouchKeyCode;        
        private static KeyCode proneKeyCode;
        private static KeyCode sprintKeyCode;
        private static KeyCode inventoryKeyCode;
        private static KeyCode interactionKeyCode;
        private static KeyCode reloadKeyCode;

        internal static double GetMouseSensivity() => mouseSensivity;

        internal static void SetMouseSensivity(double value) => mouseSensivity = value;        
    }
}