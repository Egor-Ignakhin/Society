using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SMG
{
    public class SMGMain : MonoBehaviour
    {
        [SerializeField] private LayerMask myLayerMask;
        [SerializeField] private GameObject renderObjects;
        [SerializeField] private GameObject GunMaps;

        private bool isActive;
        [SerializeField]
        private SMGCamera MSMG;
        private Camera MSMGCamera;
        private Canvas mCanvas;

        [SerializeField]
        private Transform ModifiersCellsData;
        [SerializeField]
        private Transform GunsCellsData;
        public SMGEventReceiver EventReceiver { get; private set; }       

        [SerializeField]
        private GameObject modifiersAnswer;

        [SerializeField]
        private SMGModifiersCellDescription modifiersCellDescription;

        [SerializeField]
        [ColorUsage(true, true)]
        private Color selectableColor;

        [SerializeField]
        [ColorUsage(true, true)]
        private Color defColor;

        [SerializeField]
        private SMGElementsSupport elementsSupport;
        private void Awake()
        {
            EventReceiver = new SMGEventReceiver(ModifiersCellsData, GunsCellsData, modifiersAnswer,
                FindObjectOfType<Inventory.InventoryContainer>(), MSMG, FindObjectOfType<SMGModifiersData>(), modifiersCellDescription);
        }
        private void Start()
        {
            SetEnableMaps(false);
            MSMGCamera = MSMG.GetComponent<Camera>();
            mCanvas = GetComponent<Canvas>();
        }
        public void SetEnableMaps(bool v)
        {
            GunMaps.SetActive(isActive = v);
            Cursor.visible = isActive;
            if (isActive)
            {
                Cursor.lockState = CursorLockMode.None;
                InputManager.LockInput();
                EventReceiver.OnEnable();
                SetEnableCanvasesAndCameras();
                MSMG.gameObject.SetActive(isActive);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                InputManager.Unlock();
                MSMG.gameObject.SetActive(isActive);
                SetEnableCanvasesAndCameras();
            }
        }
        public void SetEnableSMGCam(bool v)
        {
            renderObjects.SetActive(v);
            GunMaps.SetActive(!v);
            if (v)
                MSMG.Enable();
            else
                MSMG.Disable();
        }
        public void SetEnableCanvasesAndCameras()
        {
            if (!MSMGCamera)
                return;

            var canvases = FindObjectsOfType<Canvas>();
            foreach (var c in canvases)
                c.enabled = !isActive;
            var cams = FindObjectsOfType<Camera>();
            foreach (var c in cams)
                c.enabled = !isActive;
            var volumes = FindObjectsOfType<UnityEngine.Rendering.Volume>().ToList();
            foreach (var vol in volumes)
                vol.gameObject.SetActive(!isActive);

            MSMG.gameObject.SetActive(isActive);
            MSMGCamera.enabled = isActive;

            mCanvas.enabled = true;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (lastSelectedObj && !elementsSupport.IsActive)
                {
                    elementsSupport.Show();
                }
            }
            if (!MSMG.IsActive)
                return;
            MSMG.RotateAroundMouse();
        }
        private MeshRenderer lastSelectedObj;
        private void FixedUpdate()
        {
            if (MSMG.IsActive)
            {
                return;
            }
            if (!isActive)
                return;
            Ray ray = MSMGCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, myLayerMask, QueryTriggerInteraction.Ignore))
            {
                MeshRenderer currentMesh = hit.transform.GetComponent<MeshRenderer>();
                if (lastSelectedObj && currentMesh != lastSelectedObj)
                    lastSelectedObj.material.SetColor("_EmissionColor", defColor);

                if (currentMesh != lastSelectedObj)
                    currentMesh.material.SetColor("_EmissionColor", selectableColor);
                lastSelectedObj = currentMesh;
            }
            else
            {
                if (lastSelectedObj && !elementsSupport.IsActive)
                {
                    DeselectGunElement();
                }
            }
        }
        public void DeselectGunElement()
        {
            lastSelectedObj.material.SetColor("_EmissionColor", defColor);
            lastSelectedObj = null;
        }
        internal void UnequipGunElement()
        {
            GunModifiersActiveManager gmam = lastSelectedObj.transform.GetComponentInParent<GunModifiersActiveManager>();
            gmam.SetMag(ModifierCharacteristics.ModifierIndex.None);
            EventReceiver.UnequipMagOnSelGun();
            DeselectGunElement();
        }        
        private void OnDisable()
        {
            EventReceiver.OnDisable();
        }
    }
}
