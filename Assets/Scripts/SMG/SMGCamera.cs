using UnityEngine;

namespace SMG
{
    public class SMGCamera : MonoBehaviour
    {
        /// <summary>
        /// шаг камеры при приближении-отдалении
        /// </summary>
        private const int cameraScrollStep = 5;
        private float defCamFov;
        [SerializeField]
        private Transform activeGun;
        private Vector3 oldPos;

        private Quaternion activeGunDefRot;

        [SerializeField]
        private Camera mCamera;
        private void Awake()
        {
            activeGunDefRot = activeGun.rotation;
            defCamFov = mCamera.fieldOfView;
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