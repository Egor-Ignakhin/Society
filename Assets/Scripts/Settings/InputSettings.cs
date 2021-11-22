
using UnityEngine;

namespace Society.Settings
{
    internal static class InputSettings
    {
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

    }
}