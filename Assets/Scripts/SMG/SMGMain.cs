﻿using System.Collections.Generic;
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
        private SMGEventReceiver eventReceiver;
        [SerializeField]
        private GameObject modifiersAnswer;

        [SerializeField]
        private SMGGunCharsDrawer gunCharsDrawer;

        private void Start()
        {
            SetEnableMaps(false);
            MSMGCamera = MSMG.GetComponent<Camera>();
            mCanvas = GetComponent<Canvas>();

            eventReceiver = new SMGEventReceiver(ModifiersCellsData, GunsCellsData, modifiersAnswer, FindObjectOfType<Inventory.InventoryContainer>(),gunCharsDrawer);
        }
        public void SetEnableMaps(bool v)
        {
            GunMaps.SetActive(v);
            if (v)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                InputManager.LockInput();
                MSMG.gameObject.SetActive(true);
                eventReceiver.OnEnable();
            }
            else if (!renderObjects.activeInHierarchy)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                InputManager.Unlock();
                MSMG.gameObject.SetActive(false);
            }
        }
        public void SetEnableSMGCam(bool v)
        {
            isActive = v;
            renderObjects.SetActive(isActive);
            GunMaps.SetActive(!isActive);

            SetEnableCanvasesAndCameras(isActive);
        }
        private void SetEnableCanvasesAndCameras(bool v)
        {
            var canvases = FindObjectsOfType<Canvas>();
            foreach (var c in canvases)
                c.enabled = !v;
            var cams = FindObjectsOfType<Camera>();
            foreach (var c in cams)
                c.enabled = !v;

            MSMG.gameObject.SetActive(v);
            MSMGCamera.enabled = v;

            mCanvas.enabled = true;
        }
        private void Update()
        {
            eventReceiver.OnUpdate();

            if (!isActive)
                return;
            MSMG.RotateAroundMouse();
        }        
    }
}
