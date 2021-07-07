using UnityEngine;
using Inventory;
using System.Collections.Generic;

class BinoculeManager : MonoBehaviour
{
    [SerializeField] private GameObject bin;
    private bool possibleActive;
    private bool isActive;
    private Camera mainCam;
    private const float additionalFov = -15;
    private bool isStopped;
    private int iterator = 1;
    private const int MaxIterator = 4;

    private FirstPersonController fps;
    private Vector3 oldMousePos;
    Vector3 camAnglesAdditional = Vector3.zero;
    private ToolsAnimator toolsAnimator;
    [SerializeField] private RectTransform focusPoint;
    [SerializeField] private List<RectTransform> pointsData = new List<RectTransform>();

    private void Start()
    {
        toolsAnimator = FindObjectOfType<ToolsAnimator>();
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
            toolsAnimator.EnableBinocularsHUD();
            isStopped = false;
            bin.SetActive(isActive = true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            isStopped = true;
            iterator = 1;
        }
        if (isStopped)
        {
            if (Mathf.Approximately(mainCam.fieldOfView, GameSettings.FOV() + additionalFov * iterator))
            {
                bin.SetActive(isActive = false);
                isStopped = false;
                toolsAnimator.DisableBinocularsHUD();
            }
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
                x = Random.Range(-0.2f, 0.2f);
                y = Random.Range(-0.2f, 0.2f);
                camAnglesAdditional = new Vector3(x, y, 0);
                time = 0;
            }
            fps.Rocking(camAnglesAdditional * Time.deltaTime);
        }
        focusPoint.position = Vector3.MoveTowards(focusPoint.position, pointsData[iterator - 1].position, 10);
    }
    private void OnDestroy()
    {
        InventoryEventReceiver.ChangeSelectedCellEvent -= SetEnable;
    }
}
