using UnityEngine;

public sealed class BedController : MonoBehaviour, IGameScreen
{
    private BedManager lastBedManager;// активная система кроватей
    private BedMesh lastBedMesh;// активная кровать 
    private bool isSleeping;
    private Transform currentParent;
    /// <summary>
    /// запись состояния в контроллёр
    /// </summary>
    /// <param name="s"></param>
    /// <param name="manager"></param>
    /// <param name="bMesh"></param>
    internal void SetState(State s, BedManager manager = null, BedMesh bMesh = null)
    {
        // запись состояния во время начала сна
        if (manager != null)
        {
            lastBedManager = manager;
        }
        if (bMesh != null)
        {
            lastBedMesh = bMesh;
        }
        // конец записи состояния

        switch (s)
        {
            case State.unlocked:// в случае поднятия с кровати
                lastBedManager.RiseUp(lastBedMesh);// заправить кровать
                lastBedMesh = null;
                lastBedManager = null;
                ScreensManager.SetScreen(null);
                isSleeping = false;
                currentParent = null;
                break;

            case State.locked:// в случае укладывания в кровать
                ScreensManager.SetScreen(this, false);
                isSleeping = true;
                currentParent = bMesh.GetSleepPlace();
                break;
        }
    }
    private float sensitivity;
    private readonly float minimumVert = -45.0f;
    private readonly float maximumVert = 45.0f;

    private void Start()
    {
        sensitivity = GameSettings.GetSensivity();
    }
    private void Update()
    {
        if (isSleeping)
        {
            float rotationX = Input.GetAxis("Mouse Y") * sensitivity;
            float rotationY = Input.GetAxis("Mouse X") * sensitivity;

            currentParent.localEulerAngles += new Vector3(0, rotationY, rotationX);
            Vector3 cAngles = currentParent.eulerAngles;
            if (cAngles.z < 315 && cAngles.z > maximumVert * 2)
                currentParent.eulerAngles = new Vector3(cAngles.x, cAngles.y, minimumVert);
            if (cAngles.z < maximumVert * 2 && cAngles.z > maximumVert)
                currentParent.eulerAngles = new Vector3(cAngles.x, cAngles.y, maximumVert);
        }
    }
    public void Hide()
    {
        SetState(State.unlocked);
    }
}