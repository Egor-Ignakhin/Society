using System.Collections;
using UnityEngine;

sealed class PlayerDamageZone : MonoBehaviour
{
    [SerializeField] private float waitForNewDamage = 1;
    private float currentWFND = 0;

    [SerializeField] private float powerDamage = 1;
    private PlayerClasses.BasicNeeds basicNeeds;
    private void OnTriggerStay(Collider other)
    {
        if (basicNeeds)
        {
            if (currentWFND <= 0)
            {
                basicNeeds.InjurePerson(powerDamage);
                currentWFND = waitForNewDamage;
            }
            currentWFND -= Time.fixedDeltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<PlayerClasses.BasicNeeds>(out var bn))
        {
            currentWFND = 0; 
            basicNeeds = bn;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        basicNeeds = null;
    }
}
