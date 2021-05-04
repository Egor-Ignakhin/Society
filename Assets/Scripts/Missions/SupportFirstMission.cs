using UnityEngine;

public sealed class SupportFirstMission : MonoBehaviour
{
    [SerializeField] private bool itskillingDog;
    private FirstMission mMission;

    private void Start()
    {
        mMission = FindObjectOfType<FirstMission>();
        if (itskillingDog)
            GetComponent<DogEnemy>().UVariables.ChangeHealthEvent += ReportKillingDog;
    }
    private void ReportKillingDog(float healthDog)
    {
        if (healthDog <= Enemy.UniqueVariables.MinHealth)
        {
            mMission.KillTheDogs();
        }
    }
    private void OnDisable()
    {
        if (itskillingDog)
            GetComponent<DogEnemy>().UVariables.ChangeHealthEvent -= ReportKillingDog;
    }
}
