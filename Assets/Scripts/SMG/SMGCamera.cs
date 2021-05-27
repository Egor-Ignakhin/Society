using Shoots;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SMG
{
    public class SMGCamera : MonoBehaviour
    {
        /// <summary>
        /// шаг камеры при приближении-отдалении
        /// </summary>

        [SerializeField] private List<Transform> guns = new List<Transform>(3);

        private const int cameraScrollStep = 5;
        private float defCamFov;
        private Transform activeGun;
        private Vector3 oldPos;

        private Dictionary<Transform, Quaternion> DefGunsDefRot = new Dictionary<Transform, Quaternion>();

        [SerializeField]
        private Camera mCamera;
        [SerializeField] Volume volume;
        [SerializeField] private Transform gunsContainer;

        public bool IsActive { get; private set; }

        private void Awake()
        {
            activeGun = guns[0];
            foreach (var g in guns)
                DefGunsDefRot.Add(g, g.rotation);
            defCamFov = mCamera.fieldOfView;
            foreach (var g in guns)
                g.gameObject.SetActive(false);

            Disable();
        }
        public void SetActiveGun(int id)
        {
            activeGun.gameObject.SetActive(false);
            switch (id)
            {
                case 2:
                    activeGun = guns[0];
                    break;
                case 3:
                    activeGun = guns[1];
                    break;
                case 4:
                    activeGun = guns[2];
                    break;
            }
            activeGun.gameObject.SetActive(true);
        }
        public void RotateAroundMouse()
        {
            if (Input.GetMouseButton(0))
            {
                if (oldPos != Input.mousePosition)
                {
                    float x = oldPos.x - Input.mousePosition.x;
                    float y = oldPos.y - Input.mousePosition.y;
                    y /= 5;
                    x /= 5;

                    activeGun.Rotate(0, x, 0);
                    gunsContainer.Rotate(-y, 0, 0);
                }
            }
            oldPos = Input.mousePosition;

            if (Input.mouseScrollDelta.y > 0)
                CameraMove(false);
            else if (Input.mouseScrollDelta.y < 0)
                CameraMove(true);
        }

        internal void Enable()
        {
            ResetGunRotation();
            IsActive = true;
            volume.gameObject.SetActive(true);
        }
        internal void Disable()
        {
            ResetGunRotation();
            IsActive = false;
            volume.gameObject.SetActive(true);
        }

        private void CameraMove(bool fw)
        {
            mCamera.fieldOfView += fw ? cameraScrollStep : -cameraScrollStep;

            mCamera.fieldOfView = Mathf.Clamp(mCamera.fieldOfView, 30, 80);
        }

        internal Transform GetActiveGun()
        {
            return activeGun;
        }

        /// <summary>
        /// очищает ротацию оружия
        /// </summary>
        private void ResetGunRotation()
        {
            foreach (var g in guns)
                g.localRotation = DefGunsDefRot[g];
            gunsContainer.rotation = Quaternion.identity;
            mCamera.fieldOfView = defCamFov;
        }
    }
}