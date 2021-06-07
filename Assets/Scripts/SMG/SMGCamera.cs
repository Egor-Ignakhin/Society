using System.Collections.Generic;
using UnityEngine;

namespace SMG
{
    /// <summary>
    /// камера которая рендерит оружия на верстаке, а также она отвечает за режим предпросмотра
    /// </summary>
    class SMGCamera : MonoBehaviour
    {
        /// <summary>
        /// шаг камеры при приближении-отдалении
        /// </summary>

        [SerializeField] private List<Transform> guns = new List<Transform>(3);//оружия
        private readonly List<GunModifiersActiveManager> gunsModsMgs = new List<GunModifiersActiveManager>(3);// менеджеры модификаций на оружиях

        private const int cameraScrollStep = 5;// сила приближения-отдаления камеры
        private float defCamFov;// стандартное приближение камеры
        private Transform activeGun;// активное оружие
        private GunModifiersActiveManager activeManager;// активный мод. менеджер оружия
        private Vector3 oldPos;

        private readonly Dictionary<Transform, Quaternion> DefGunsDefRot = new Dictionary<Transform, Quaternion>();// стандартные ротации оружий
        
        private Camera mCamera;
        [SerializeField] private Transform gunsContainer;// контейнер оружия (именно на верстаке)            
        public bool IsActive { get; private set; }

        private void Awake()
        {            
            mCamera = GetComponent<Camera>();
            defCamFov = mCamera.fieldOfView;
            foreach (var g in guns)
            {
                DefGunsDefRot.Add(g, g.rotation);
                g.gameObject.SetActive(false);
                gunsModsMgs.Add(g.GetComponentInChildren<GunModifiersActiveManager>());
            }
            
            activeGun = guns[0];
            activeManager = gunsModsMgs[0];
            SetEnable(false);
        }        
        public void SetActiveGun(Inventory.InventoryCell ic)
        {            
            activeGun.gameObject.SetActive(false);
            int index = -1;
            switch (ic.Id)
            {
                case 2:
                    index = 0;
                    break;
                case 3:                    
                    index = 1;
                    break;
                case 4:
                    index = 2;                    
                    break;
            }
            activeGun = guns[index];
            activeManager = gunsModsMgs[index];
            activeGun.gameObject.SetActive(true);
        }

        internal void AddOrRemoveEvents(SMGEventReceiver ev, bool v)
        {            
            if (v)
            {                
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

        internal void SetMagToActiveGun(ModifierCell gc) => activeManager.SetMag((ModifierCharacteristics.ModifierIndex)gc.Ic.MGun.Mag);
    }
}