using UnityEngine;

public sealed class BedManager : MonoBehaviour
{
    private Transform playerT;
    private FirstPersonController fps;// игрок
    private Vector3 sleepAngles = new Vector3(-30, 90, 0);// положение во сне
    private Transform lastPlayerParent;
    //координаты игрока до сна
    private Vector3 lastPlayerLocalEulerAngles;
    private Vector3 lastPlayerPosition;
    private void Awake()
    {
        playerT = Camera.main.transform;
        fps = FindObjectOfType<FirstPersonController>();
    }
    internal void Interact(BedMesh b)
    {
        StraightenBed(b);
    }
    /// <summary>
    /// заправить кровать
    /// </summary>
    /// <param name="b"></param>
    private void StraightenBed(BedMesh b)
    {
        if (b.IsOccupied)// если кровать занята
        {
            //Возвращение позиций в исходное значение
            playerT.SetParent(lastPlayerParent);
            playerT.localEulerAngles = lastPlayerLocalEulerAngles;
            playerT.position = lastPlayerPosition;
            playerT.localScale = Vector3.one;
            //конец возвращений позиций            
            b.SetOccupied(false);// кровать больше не занята
            return;
        }
        //иначе если кровать свободна

        //перемещение позиций в спальное значение
        lastPlayerParent = playerT.parent;
        playerT.SetParent(b.transform);

        lastPlayerLocalEulerAngles = playerT.localEulerAngles;
        playerT.localEulerAngles = sleepAngles;

        lastPlayerPosition = playerT.position;
        playerT.position = b.SleepPlace.position;
        //конец перемещения позиций
        
        playerT.GetComponent<BedController>().SetState(State.locked, this, b);
    }
    /// <summary>
    /// поднятся с кровати
    /// </summary>
    /// <param name="bedMesh"></param>
    public void RiseUp(BedMesh bedMesh)
    {
        StraightenBed(bedMesh);
    }
}
