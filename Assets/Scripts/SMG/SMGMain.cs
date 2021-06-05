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

            mCanvas.enabled = true;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (lastSelectedObj && !DEA.IsActive)
                    DEA.Show(UnequipGunElement, DeselectGunElement, "Снять модификатор?");
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
