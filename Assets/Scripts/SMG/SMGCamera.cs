using Shoots;
using System.Collections.Generic;
using UnityEngine;

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

        private Quaternion activeGunDefRot;

        [SerializeField]
        private Camera mCamera;
        private void Awake()
        {
            activeGun = guns[0];
            activeGunDefRot = activeGun.rotation;
            defCamFov = mCamera.fieldOfView;
            foreach (var g in guns)
                g.gameObject.SetActive(false);
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
        private void OnEnable()
        {
            ResetGunRotation();
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
                    activeGun.parent.Rotate(-y, 0, 0);
                }
            }
            oldPos = Input.mousePosition;

            if (Input.mouseScrollDelta.y > 0)
                CameraMove(false);
            else if (Input.mouseScrollDelta.y < 0)
                CameraMove(true);
        }

        private void CameraMove(bool fw)
        {
            mCamera.fieldOfView += fw ? cameraScrollStep : -cameraScrollStep;

            mCamera.fieldOfView = Mathf.Clamp(mCamera.fieldOfView, 30, 80);
        }

        /// <summary>
        /// очищает ротацию оружия
        /// </summary>
        private void ResetGunRotation()
        {
            activeGun.rotation = activeGunDefRot;
            mCamera.fieldOfView = defCamFov;
        }
    }
}