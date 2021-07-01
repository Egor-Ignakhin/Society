using UnityEngine;

public sealed class ChairManager : MonoBehaviour
{
    private Transform playerT;    
    private Vector3 seatAngles = new Vector3(0, 90, 0);// сидячее положение
    private Transform lastPlayerParent;
    //координаты игрока до посадки
    private Vector3 lastPlayerLocalEulerAngles;
    private Vector3 lastPlayerPosition;
    private SeatController seatController;
    private void Awake()
    {
        playerT = Camera.main.transform;        
        seatController = FindObjectOfType<SeatController>();
    }
    internal void Interact(ChairMesh c)
    {
        seatController.RemoveLastChair();
        GetOutOfChair(c);        
    }

    /// <summary>
    /// оставить стул
    /// </summary>
    /// <param name="c"></param>
    private void GetOutOfChair(ChairMesh c)
    {        
        if (c.IsOccupied)// если стул занят
        {
            DeOccupied(c);
            return;
        }
        //иначе если стул свободен

        //перемещение позиций в сидячее значение
        lastPlayerParent = playerT.parent;
        playerT.SetParent(c.GetSeatPlace());

        lastPlayerLocalEulerAngles = playerT.localEulerAngles;
        playerT.localEulerAngles = seatAngles;

        lastPlayerPosition = playerT.position;
        playerT.position = c.GetSeatPlace().position;
        //конец перемещения позиций
        
        seatController.SetState(State.locked, this, c);
    }
    /// <summary>
    /// поднятся со стула
    /// </summary>
    /// <param name="cMesh"></param>
    public void RiseUp(ChairMesh cMesh) => GetOutOfChair(cMesh);

    public void DeOccupied(ChairMesh c)
    {        
        //Возвращение позиций в исходное значение
        playerT.SetParent(lastPlayerParent);
        playerT.localEulerAngles = lastPlayerLocalEulerAngles;
        playerT.position = lastPlayerPosition;
        //конец возвращений позиций        
        c.SetOccupied(false);// стул больше не занят
    }
}
