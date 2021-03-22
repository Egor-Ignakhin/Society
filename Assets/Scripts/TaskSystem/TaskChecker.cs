using UnityEngine;

/// <summary>
/// класс вызывает при контакте событие чекпоинта
/// </summary>
public sealed class TaskChecker : MonoBehaviour
{
    [SerializeField] private Mission mMission;
    [SerializeField] private MonoBehaviour target;
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
