using Society.Patterns;
using Society.Player.Controllers;

using UnityEngine;

namespace Society.SMG
{
    /// <summary>
    /// класс отвечающий за верстак
    /// </summary>
    internal sealed class WorkbenchSMG : InteractiveObject
    {
        private SMGMain main;
        [SerializeField] private Transform cameraPoint;
        private void Start()
        {
            main = FindObjectOfType<SMGMain>();
            SetType("Workbench");
            if (!cameraPoint)
                Debug.LogError("Not camera point!");
        }
        public override void Interact()
        {
            if (!Society.GameScreens.ScreensManager.HasActiveScreen())
            {
                main.SetEnable(true);
                var camTr = FindObjectOfType<FirstPersonController>().GetCamera().transform;
                camTr.SetParent(cameraPoint);
                camTr.localPosition = Vector3.zero;
                camTr.localRotation = Quaternion.identity;
            }
        }
    }
}