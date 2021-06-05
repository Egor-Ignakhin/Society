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
        private readonly List<GunModifiersActiveManager> gunsModsMgs = new List<GunModifiersActiveManager>(3);

        private const int cameraScrollStep = 5;
        private float defCamFov;
        private Transform activeGun;
        private GunModifiersActiveManager activeManager;
        private Vector3 oldPos;

        private readonly Dictionary<Transform, Quaternion> DefGunsDefRot = new Dictionary<Transform, Quaternion>();

        [SerializeField]
        private Camera mCamera;
        [SerializeField] private Transform gunsContainer;       
        private Inventory.InventoryEventReceiver inventoryEventReceiver; 
        public bool IsActive { get; private set; }

        private void Awake()
        {
            foreach (var g in guns)
                DefGunsDefRot.Add(g, g.rotation);
            defCamFov = mCamera.fieldOfView;
            foreach (var g in guns)
                g.gameObject.SetActive(false);

            foreach (var g in guns)
                gunsModsMgs.Add(g.GetComponentInChildren<GunModifiersActiveManager>());
            activeGun = guns[0];
            activeManager = gunsModsMgs[0];
            SetEnable(false);
        }        
        public void SetActiveGun(Inventory.InventoryCell ic)
        {            
            activeGun.gameObject.SetActive(false);
            switch (ic.Id)
            {
                case 2:
                    activeGun = guns[0];
                    activeManager = gunsModsMgs[0];
                    break;
                case 3:
                    activeGun = guns[1];
                    activeManager = gunsModsMgs[1];
                    break;
                case 4:
                    activeGun = guns[2];
                    activeManager = gunsModsMgs[2];
                    break;
            }
            activeGun.gameObject.SetActive(true);
        }

        internal void AddOrRemoveEvents(SMGEventReceiver ev, bool v)
        {            
            if (v)
            {
                inventoryEventReceiver = FindObjectOfType<Inventory.InventoryContainer>().EventReceiver;
                ev.UpdateModfiersEvent += SetMagToActiveGun;
                ev.ChangeGunEvent += SetActiveGun;
            }
            else
            {                
                ev.UpdateModfiersEvent -= SetMagToActiveGun;
                ev.ChangeGunEvent -= SetActiveGun;
            }
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

        public void SetEnable(bool v)
        {            
            ResetGunRotation();
            IsActive = v;
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
            foreach (var g in guns)
                g.localRotation = DefGunsDefRot[g];
            gunsContainer.rotation = Quaternion.identity;
            mCamera.fieldOfView = defCamFov;
        }

        internal void SetMagToActiveGun(SMGModifiersCell gc) => activeManager.SetMag((ModifierCharacteristics.ModifierIndex)gc.Ic.MGun.Mag);
    }
}