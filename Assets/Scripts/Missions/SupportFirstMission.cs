using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SupportFirstMission : MonoBehaviour
{
    [SerializeField] private bool itskillingDog;
    private FirstMission mMission;

    private void Awake()
    {
        mMission = FindObjectOfType<FirstMission>();
    }
    private void OnEnable()
    {
        if (itskillingDog)
            GetComponent<DogEnemy>().ChangeHealthEvent += ReportKillingDog;
    }
    private void ReportKillingDog(float healthDog)
    {
        if(healthDog <= Enemy.MinHealth)
        {
            mMission.KillTheDogs();
        }
    }
    private void OnDisable()
    {
        if (itskillingDog)
            GetComponent<DogEnemy>().ChangeHealthEvent -= ReportKillingDog;
    }    
}
