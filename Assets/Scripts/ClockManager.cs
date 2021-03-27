using UnityEngine;

public sealed class ClockManager : MonoBehaviour
{
    private Times.WorldTime worldTime;
    [SerializeField] private TMPro.TextMeshPro text;
    [SerializeField] private bool isElectronicClock;
    [SerializeField] private Transform[] pointers = new Transform[2];
    [SerializeField] private Vector3 additionalRotateForMin;
    [SerializeField] private Vector3 additionalRotateForHours;
    private void OnEnable()
    {
        worldTime = FindObjectOfType<Times.WorldTime>();
        if (isElectronicClock)
            worldTime.ChangeTimeEvent += RenderOnText;
        else
            worldTime.ChangeTimeEventInNumbers += RenderOnPointers;
    }

    private void RenderOnText(string value)
    {
        text.SetText(value);
    }
    private void RenderOnPointers(int s, int m, int h)
    {
        float min = m;
        float hours = h;
        float sec = s;
        // поворот всех стрелок относительно текущего времени
        pointers[0].localRotation = Quaternion.Euler(new Vector3(0, min / 60 * 360 + sec / 10, 0) + additionalRotateForMin);
        pointers[1].localRotation = Quaternion.Euler(new Vector3(0, hours * 30 + min / 2, 0) + additionalRotateForHours);
    }
    private void OnDisable()
    {
        if (isElectronicClock)
            worldTime.ChangeTimeEvent -= RenderOnText;
        else
            worldTime.ChangeTimeEventInNumbers -= RenderOnPointers;
    }
}
