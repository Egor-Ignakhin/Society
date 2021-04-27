using System.Collections.Generic;
using UnityEngine;

public sealed class PickUpAndDropDrawer : MonoBehaviour
{
    private const int slotsCount = 3;// количество слотов
    public static int SlotsCount => slotsCount;
    [SerializeField] private List<SlotForAddingItems> slotsOnScene = new List<SlotForAddingItems>(SlotsCount);// сами слоты на сцене

    private float timeToLightening = 3;// время до потемнения слота
    private float[] currentTimeToLightening = new float[SlotsCount];// текущее время для каждого слота
    private float speedLightening = 0.625f;// скорость потемнения
    private float shiftY = 90;// высота, на которую возвисится старый слот, при добавлении к коллекцию нового
    private readonly List<int> movableSlots = new List<int>();// движимые слоты    

    /// <summary>
    /// отрисовка нового предмета
    /// </summary>
    /// <param name="type"></param>
    /// <param name="count"></param>
    public void DrawNewItem(string type, int count)
    {
        if (movableSlots.Count >= SlotsCount)
        {
            GetSlot();
        }
            
        TranslateLabels();

        slotsOnScene[movableSlots.Count].SetText(type, count);
        currentTimeToLightening[movableSlots.Count] = timeToLightening * 2;
        slotsOnScene[movableSlots.Count].ReturnDefaultColor();
        slotsOnScene[movableSlots.Count].SetDefaultPosition();
        movableSlots.Add(movableSlots.Count);
    }
    private void Update()
    {
        LighteningBackground();
    }

    /// <summary>
    /// постепенное затемнение слота
    /// </summary>
    private void LighteningBackground()
    {
        if (movableSlots.Count == 0)
            return;               

        foreach (int m in movableSlots)
        {
            float difference = speedLightening * Time.deltaTime;
            if ((currentTimeToLightening[m] -= difference) < timeToLightening)
            {
                Color c = new Color(0, 0, 0, difference);
                slotsOnScene[m].DifferenceColor(c);
            }
        }
        movableSlots.RemoveAll(elem => slotsOnScene[elem].DisableSlot());
    }
    /// <summary>
    /// перемещение слотов
    /// </summary>
    private void TranslateLabels()
    {
        movableSlots.Reverse();
        foreach (int m in movableSlots)
        {
            slotsOnScene[m].Translate(movableSlots.IndexOf(m) + 1, shiftY);
        }
    }
    private void GetSlot()
    {
        movableSlots.RemoveAt(0);
    }
}
