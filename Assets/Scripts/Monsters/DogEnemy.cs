using PlayerClasses;
using UnityEngine;
using UnityEngine.AI;

public sealed class DogEnemy : Enemy
{
    [SerializeField] private Transform head;
    private void OnEnable()
    {
        base.Init(2, 3, 15, 100);      
    }
    protected override void LookOnTarget()
    {
        Vector3 startRot = transform.localEulerAngles;
        transform.LookAt(currentEnemy ? currentEnemy.transform : currentTarget);
        transform.localEulerAngles = new Vector3(startRot.x, transform.localEulerAngles.y, 0);
        head.transform.LookAt(currentEnemy.transform);
    }

    protected override string Type()
    {
        return TypesEnemies.BloodDog;
    }
}
