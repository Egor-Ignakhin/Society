using UnityEngine;

public class SeatController : Singleton<SeatController>, IState
{
    public State CurrentState { get; set; }
    private bool isSitting;
    private KeyCode pressToStay = KeyCode.Space;// клавиша для того, чтобы встать
    private ChairManager lastChairManager;// активная система стульев
    private ChairMesh lastChairMesh;// активный стул
    private Times.WorldTime worldTime;
    private Camera playerCamera;

    private void Awake()
    {
        worldTime = FindObjectOfType<Times.WorldTime>();
        playerCamera = Camera.main;
    }
    /// <summary>
    /// запись состояния в контроллёр
    /// </summary>
    /// <param name="s"></param>
    /// <param name="manager"></param>
    /// <param name="bMesh"></param>
    internal void SetState(State s, ChairManager manager = null, ChairMesh cMesh = null)
    {
        // запись состояния во время начала сна
        if (manager != null)
        {
            lastChairManager = manager;
        }
        if (cMesh != null)
        {
            lastChairMesh = cMesh;
        }
        // конец записи состояния

        switch (s)
        {
            case State.unlocked:// в случае поднятия с кровати
                isSitting = false;
                lastChairManager.RiseUp(lastChairMesh);// заправить кровать               
                worldTime.ReduceSpeed(lastChairMesh.GetTimeMultiply());// возвращение скорости времени к обычному состоянию
                lastChairMesh = null;
                lastChairManager = null;
                break;

            case State.locked:// в случае укладывания в кровать
                isSitting = true;
                worldTime.IncreaseSpeed(lastChairMesh.GetTimeMultiply());// повышение скорости времени
                break;
        }
    }


    private float sensitivityHor = 3;
    private float sensitivityVert = 3;
    private float minimumVert = -45.0f;
    private float maximumVert = 45.0f;
    private float rotationX = 0;


    private void Update()
    {
        if (isSitting)
        {
            if (Input.GetKeyDown(pressToStay))
            {
                SetState(State.unlocked);
            }

            rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert);
            float delta = Input.GetAxis("Mouse X") * sensitivityHor;
            float rotationY = transform.localEulerAngles.y + delta;
            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
    }

    public static void RemoveLastChair()
    {
        if (Instance.lastChairManager != null && Instance.lastChairMesh != null)
            Instance.lastChairManager.DeOccupied(Instance.lastChairMesh);
    }
}
