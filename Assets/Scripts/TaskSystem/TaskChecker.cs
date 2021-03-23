using PlayerClasses;
using UnityEngine;

/// <summary>
/// класс вызывает при контакте событие чекпоинта
/// </summary>
public sealed class TaskChecker : InteractiveObject
{
    [SerializeField] private Mission mMission;
    [SerializeField] private MonoBehaviour target;

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
    private void Report()
    {
        mMission.Report();
        gameObject.SetActive(false);
    }
}
