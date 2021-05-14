using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

class BinoculeManager : MonoBehaviour
{
    [SerializeField] private GameObject bin;
    private bool possibleActive;
    private bool isActive;
    private Camera mainCam;
    private const float additionalFov = -20;
    private int iterator;
    private const int MaxIterator = 3;

    private FirstPersonController fps;
    private Vector3 oldMousePos;
    Vector3 camAnglesAdditional = Vector3.zero;

    private void Start()
    {
        fps = FindObjectOfType<FirstPersonController>();
        mainCam = Camera.main;
        InventoryEventReceiver.ChangeSelectedCellEvent += SetEnable;
        bin.SetActive(false);
    }
    private void SetEnable(int id)
    {
        possibleActive = id == 7;
        if (!possibleActive)
        {
            bin.SetActive(isActive = false);
            mainCam.fieldOfView = GameSettings.FOV();
            iterator = 1;            
        }
        time = 0;
        camAnglesAdditional = Vector3.zero;
    }
    private void Update()
    {
        if (!possibleActive)
            return;
        if (Input.GetMouseButtonDown(1))
        {
            bin.SetActive(isActive = true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            bin.SetActive(isActive = false);
            iterator = 1;
        }
        Animate();
        if (Input.mousePosition != oldMousePos)
        {
            time = 0;
            camAnglesAdditional = Vector3.zero;            
        }
        oldMousePos = Input.mousePosition;
    }
    float time = 0;
    float x = 0;
    float y = 0;
    private void Animate()
    {
        float targetFov = isActive ? GameSettings.FOV() + additionalFov * iterator : GameSettings.FOV();
        mainCam.fieldOfView = Mathf.MoveTowards(mainCam.fieldOfView, targetFov, 1);
        InventoryEventReceiver.LockScrollEvent(isActive);
        if (isActive)
        {
            if (Input.mouseScrollDelta.y > 0)
                iterator++;
            if (Input.mouseScrollDelta.y < 0)
                iterator--;
            if (iterator >= MaxIterator)
                iterator = MaxIterator;
            if (iterator < 1)
                iterator = 1;

            if ((time += Time.deltaTime) > 3)
            {
                x = Random.Range(-.2f, .2f);
                y = Random.Range(-.2f, .2f);
                camAnglesAdditional = new Vector3(x, y, 0);
                time = 0;
            }
            fps.SmoothRocking(camAnglesAdditional * Time.deltaTime);
        }
    }
    private void OnDestroy()
    {
        InventoryEventReceiver.ChangeSelectedCellEvent -= SetEnable;
    }
}
