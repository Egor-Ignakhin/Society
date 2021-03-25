using PlayerClasses;
using UnityEngine;

/// <summary>
/// класс вызывает при контакте событие чекпоинта
/// </summary>
public sealed class TaskChecker : InteractiveObject
{
    [SerializeField] private Mission mMission;
    [SerializeField] private MonoBehaviour target;

    [Space(15)]
    [SerializeField] private bool delayedToInvoke;
    [ShowIf(nameof(delayedToInvoke), true)] [SerializeField] private float delayToInvoke;

    [Space(15)]
    [SerializeField] private bool EnableNextChecker;
    [ShowIf(nameof(EnableNextChecker), true)] [SerializeField] private GameObject nextChecker;

    [Space(15)]
    [SerializeField] private bool reportAfterChangeStateOfObject;
    [ShowIf(nameof(reportAfterChangeStateOfObject), true)] [SerializeField] private GameObject changedObject;

    [Space(15)]
    [SerializeField] private bool isSupportForExtrmCloseDoor;

    private bool hasInteracted;


    public override void Interact(PlayerStatements pl)
    {
        Report();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target.gameObject)
        {
            Report();
        }
    }
    private async void Report()
    {
        if (hasInteracted)
            return;
        hasInteracted = true;
        if (delayedToInvoke)
            await System.Threading.Tasks.Task.Delay((int)delayToInvoke * 1000);

        mMission.Report();

        if (EnableNextChecker)
        {
            if (isSupportForExtrmCloseDoor)
            {
                while (!(mMission as FirstMission).PossibleMoveToBunker())
                {
                    await System.Threading.Tasks.Task.Delay(100);
                }
            }
            nextChecker.SetActive(true);
        }
    }
}
