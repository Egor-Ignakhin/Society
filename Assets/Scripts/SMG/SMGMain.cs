﻿using UnityEngine;

namespace SMG
{
    public class SMGMain : MonoBehaviour, IGameScreen
    {
        [SerializeField] private LayerMask myLayerMask;
        [SerializeField] private GameObject renderObjects;
        [SerializeField] private GameObject modificationMode;

        public bool IsActive { get; private set; }
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
        private EffectsManager effectsManager;
        private void Awake()
        {
            effectsManager = FindObjectOfType<EffectsManager>();
            EventReceiver = new SMGEventReceiver(ModifiersCellsData, GunsCellsData, modifiersAnswer,
                FindObjectOfType<Inventory.InventoryContainer>(), MSMG, FindObjectOfType<SMGModifiersData>(), modifiersCellDescription);
            MSMGCamera = MSMG.GetComponent<Camera>();
            mCanvas = GetComponent<Canvas>();
            SetEnable(false);
            ScreensManager.OnInit();            
        }
        public void SetEnable(bool v)
        {
            IsActive = v;
            modificationMode.SetActive(IsActive);
            SetEnableCanvasesAndCameras();
            MSMG.gameObject.SetActive(IsActive);
            EventReceiver.SetEnable(IsActive);
            ScreensManager.SetScreen(IsActive ? this : null);
        }
        public void SetEnableSMGCam(bool v)
        {
            renderObjects.SetActive(v);
            modificationMode.SetActive(!v);
            MSMG.SetEnable(v);
        }
        public void SetEnableCanvasesAndCameras()
        {
            var canvases = FindObjectsOfType<Canvas>();
            foreach (var c in canvases)
                c.enabled = !IsActive;
            var cams = FindObjectsOfType<Camera>();
            foreach (var c in cams)
                c.enabled = !IsActive;
            effectsManager.SetEnableAllEffects(!IsActive);

            MSMG.gameObject.SetActive(IsActive);

            mCanvas.enabled = true;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (lastSelectedObj && !elementsSupport.IsActive)
                    elementsSupport.Show();
            }
            if (!MSMG.IsActive)
                return;
            MSMG.RotateAroundMouse();
        }
        private MeshRenderer lastSelectedObj;
        private void FixedUpdate()
        {
            if (!IsActive || MSMG.IsActive)
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
                    DeselectGunElement();
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

        public void Hide()
        {
            if (MSMG.IsActive)
            {
                SetEnableSMGCam(false);
                return;
            }
            SetEnable(false);
        }
    }
}
