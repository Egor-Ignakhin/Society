using UnityEngine;

public sealed class DogEnemy : Enemy
{
    private void OnEnable()
    {
        Init(2, 3, seeDistance, health);
    }
    protected override void LookOnTarget()
    {
        Vector3 startRot = transform.localEulerAngles;
        transform.LookAt(target);
        transform.localEulerAngles = new Vector3(startRot.x, transform.localEulerAngles.y, 0);
    }

    protected override string Type()
    {
        return TypesEnemies.BloodDog;
    }
}
