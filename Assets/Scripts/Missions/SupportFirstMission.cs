using UnityEngine;

public sealed class SupportFirstMission : MonoBehaviour
{
    [SerializeField] private bool itskillingDog;
    private FirstMission mMission;

    private void Start()
    {
        return;
        mMission = FindObjectOfType<FirstMission>();
        if (itskillingDog)
            GetComponent<DogEnemy>().UVariables.ChangeHealthEvent += ReportKillingDog;
    }
    private void ReportKillingDog(float healthDog)
    {
        return;
        if (healthDog <= Enemy.UniqueVariables.MinHealth)
        {
            mMission.KillTheDogs();
        }
    }
    private void OnDisable()
    {
        return;
        if (itskillingDog)
            GetComponent<DogEnemy>().UVariables.ChangeHealthEvent -= ReportKillingDog;
    }
}
