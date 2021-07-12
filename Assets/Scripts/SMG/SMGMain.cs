using System;
using UnityEngine;

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
        private DynamicalElementsAnswer DEA;
        private EffectsManager effectsManager;
        [SerializeField] private Transform additionCellsForModifiers;
        [SerializeField] private Transform activeModifiersContainer;

        private void Awake()
        {
            effectsManager = FindObjectOfType<EffectsManager>();
            EventReceiver = new SMGEventReceiver(GunsCellsData, modifiersAnswer,
                FindObjectOfType<Inventory.InventoryContainer>(), FindObjectOfType<SMGModifiersData>(), modifiersCellDescription, additionCellsForModifiers, activeModifiersContainer, DEA);
            MSMGCamera = MSMG.GetComponent<Camera>();
            mCanvas = GetComponent<Canvas>();
            SetEnable(false);
            ScreensManager.OnInit();
        }
        public void SetEnable(bool v)
        {
            IsActive = v;
            MSMG.AddOrRemoveEvents(EventReceiver, v);
            modificationMode.SetActive(IsActive);
            SetEnableCanvasesAndCameras();
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

            Camera cam;
            if (cam = FindObjectOfType<FirstPersonController>().GetCamera())
            {
                cam.enabled = true;
                if (!IsActive)
                {
                    cam.transform.SetParent(FindObjectOfType<FirstPersonController>().transform.GetChild(0));
                }
            }

            mCanvas.enabled = true;
        }
        private void Update()
        {
            if (!IsActive)
                return;
            if (Input.GetMouseButtonDown(1))
            {
                if (lastSelectedObj && !DEA.IsActive && (!lastSelectedObj.GetComponent<SMGGunElement>().IsEmpty()))
                    DEA.Show(UnequipGunElement, DeselectGunElement, "Снять модификатор?");
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (EventReceiver.CurGunCellContAnyMod())
                {
                    DEA.Show(UnequipAllElements, null, "Вы действительно хотите разобрать оружие?");
                }
            }
            if (Input.GetKeyDown(KeyCode.V) && !MSMG.IsActive)
            {
                SetEnableSMGCam(true);
            }
            if (!MSMG.IsActive)
                return;
            MSMG.RotateAroundMouse();
        }

        private MeshRenderer lastSelectedObj;
        private void FixedUpdate()
        {
            if (!IsActive || MSMG.IsActive || DEA.IsActive)
                return;
            Ray ray = MSMGCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, myLayerMask, QueryTriggerInteraction.Ignore))
            {                
                MeshRenderer currentMesh = hit.transform.GetComponent<MeshRenderer>();
                if (lastSelectedObj && (currentMesh != lastSelectedObj))
                {
                    lastSelectedObj.material.SetColor("_EmissionColor", defColor);
                }

                if (currentMesh != lastSelectedObj)
                {
                    currentMesh.material.SetColor("_EmissionColor", selectableColor);         
                }
                lastSelectedObj = currentMesh;
            }
            else
            {
                if (lastSelectedObj && !DEA.IsActive)
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
            EventReceiver.UnequipGunElement(lastSelectedObj.GetComponent<SMGGunElement>());
            DeselectGunElement();
        }
        private void UnequipAllElements()
        {
            if (lastSelectedObj)
            {
                lastSelectedObj.material.SetColor("_EmissionColor", defColor);
                lastSelectedObj = null;
            }
            EventReceiver.UnequipAllElements();
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

        public KeyCode HideKey() => KeyCode.Escape;
    }
}
