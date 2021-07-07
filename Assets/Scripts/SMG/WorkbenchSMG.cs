using UnityEngine;
using PlayerClasses;

namespace SMG
{
    /// <summary>
    /// класс отвечающий за верстак
    /// </summary>
    class WorkbenchSMG : InteractiveObject
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
        public override void Interact(PlayerStatements pl)
        {
            if (!ScreensManager.HasActiveScreen())
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