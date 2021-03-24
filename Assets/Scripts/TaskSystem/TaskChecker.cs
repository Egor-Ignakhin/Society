using PlayerClasses;
using UnityEngine;

/// <summary>
/// класс вызывает при контакте событие чекпоинта
/// </summary>
public sealed class TaskChecker : InteractiveObject
{
    [SerializeField] private Mission mMission;
    [SerializeField] private MonoBehaviour target;

    [Space(25)]
    [SerializeField] private bool delayedToInvoke;
    [SerializeField] private float delayToInvoke;

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
    }
}
