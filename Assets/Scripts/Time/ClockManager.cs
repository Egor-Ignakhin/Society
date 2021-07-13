using Times;
using UnityEngine;

public sealed class ClockManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro text;
    [SerializeField] private bool isElectronicClock;
    [ShowIf(nameof(isElectronicClock), false)] [SerializeField] private Transform[] pointers = new Transform[2];
    [ShowIf(nameof(isElectronicClock), false)] [SerializeField] private Vector3 additionalRotateForMin;
    [ShowIf(nameof(isElectronicClock), false)] [SerializeField] private Vector3 additionalRotateForHours;
    private WorldTime worldTime;
    private void Start()
    {
        worldTime = FindObjectOfType<WorldTime>();
        if (isElectronicClock)
            worldTime.ChangeTimeEvent += RenderOnText;
        else
            worldTime.ChangeTimeEventInNumbers += RenderOnPointers;
    }
    int delayAfterLastAnim;
    private void RenderOnText(string value)
    {
        if ((++delayAfterLastAnim) > 1)
        {
            value = value.Replace(':', ' ');
            delayAfterLastAnim = 0;
        }

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
        if (!worldTime)
            return;
        if (isElectronicClock)
            worldTime.ChangeTimeEvent -= RenderOnText;
        else
            worldTime.ChangeTimeEventInNumbers -= RenderOnPointers;
    }
}
