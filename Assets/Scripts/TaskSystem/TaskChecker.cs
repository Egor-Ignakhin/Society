using System;
using UnityEngine;

/// <summary>
/// класс вызывает при контакте событие чекпоинта
/// </summary>
public sealed class TaskChecker : InteractiveObject
{
    [Flags]
    public enum MissionFlags
    {
        Nothing = 0,
        M_1 = 1,
        M_2 = 2,
        M_3 = 4,
        M_4 = 8,
        M_5 = 16,
        M_6 = 32,
        M_7 = 64,
        M_8 = 128,
        M_9 = 256,
        Everything = ~0
    }

    [EnumFlag]
    [SerializeField]
    private MissionFlags enumFlag = MissionFlags.M_1;
    [SerializeField] private int task;
    private Mission mMission;
    [SerializeField] private MonoBehaviour target;

    private bool hasInteracted;

    private void Start()
    {
        if (enumFlag == MissionFlags.M_1)
            mMission = FindObjectOfType<PrologMission>();
    }
    public override void Interact() => Report();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target.gameObject)
        {
            Report();
        }
    }
    private void Report()
    {
        //защита от нажатия не по сценарию
        if (!CanInteract())
            return;
        if (hasInteracted)
            return;
        hasInteracted = true;

        mMission.Report();
    }

    internal bool CanInteract() =>
        (mMission.GetCurrentTask() == (task - 1));

}
