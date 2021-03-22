using UnityEngine;

public sealed class ChairManager : MonoBehaviour
{
    private Transform playerT;
    private FirstPersonController fps;// игрок
    private Vector3 seatAngles = new Vector3(0, 90, 0);// сидячее положение
    private Transform lastPlayerParent;
    //координаты игрока до посадки
    private Vector3 lastPlayerLocalEulerAngles;
    private Vector3 lastPlayerPosition;
    private bool isInitialized = false;
    private void Init()
    {
        playerT = Camera.main.transform;
        fps = FindObjectOfType<FirstPersonController>();
        isInitialized = true;
    }
    internal void Interact(ChairMesh c)
    {
        GetOutOfChair(c);
    }   

    /// <summary>
    /// оставить стул
    /// </summary>
    /// <param name="c"></param>
    private void GetOutOfChair(ChairMesh c)
    {
        if (!isInitialized)
        {
            Init();
        }
        if (c.IsOccupied)// если стул занят
        {
            //Возвращение позиций в исходное значение
            playerT.SetParent(lastPlayerParent);
            playerT.localEulerAngles = lastPlayerLocalEulerAngles;
            playerT.position = lastPlayerPosition;
            //конец возвращений позиций
            fps.SetState(State.unlocked);// разблокировка персонажа
            c.SetOccupied(false);// стул больше не занят
            return;
        }
        //иначе если стул свободен

        //перемещение позиций в сидячее значение
        lastPlayerParent = playerT.parent;
        playerT.SetParent(c.transform);

        lastPlayerLocalEulerAngles = playerT.localEulerAngles;
        playerT.localEulerAngles = seatAngles;

        lastPlayerPosition = playerT.position;
        playerT.position = c.SeatPlace.position;
        //конец перемещения позиций

        fps.SetState(State.locked);// блокировка персонажа
        playerT.GetComponent<SeatController>().SetState(State.locked, this, c);
    }
    /// <summary>
    /// поднятся со стула
    /// </summary>
    /// <param name="cMesh"></param>
    public void RiseUp(ChairMesh cMesh)
    {
        GetOutOfChair(cMesh);
    }
}
