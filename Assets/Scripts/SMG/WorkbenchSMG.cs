using UnityEngine;

namespace SMG
{
    /// <summary>
    /// класс отвечающий за верстак
    /// </summary>
    sealed class WorkbenchSMG : InteractiveObject
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