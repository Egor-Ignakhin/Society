using UnityEngine;

public sealed class BedController : MonoBehaviour, IState
{
    public State CurrentState { get; set; }
    private bool isSleeping;
    private KeyCode pressToStay = KeyCode.Space;// клавиша для того, чтобы встать
    private BedManager lastBedManager;// активная система кроватей
    private BedMesh lastBedMesh;// активная кровать
    private Times.WorldTime worldTime;
    private int timeMultiply = 4;// множитель времени, на который умножается время во время сна

    private void Awake()
    {
        worldTime = FindObjectOfType<Times.WorldTime>();
    }
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
                isSleeping = false;
                lastBedManager.RiseUp(lastBedMesh);// заправить кровать
                lastBedMesh = null;
                lastBedManager = null;
                worldTime.ReduceSpeed(timeMultiply);// возвращение скорости времени к обычному состоянию
                break;

            case State.locked:// в случае укладывания в кровать
                isSleeping = true;
                worldTime.IncreaseSpeed(timeMultiply);// повышение скорости времени
                break;
        }
    }
    private void Update()
    {
        if (isSleeping)
        {
            if (Input.GetKeyDown(pressToStay))
            {
                SetState(State.unlocked);
            }
        }
    }
}