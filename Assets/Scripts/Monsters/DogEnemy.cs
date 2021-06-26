using UnityEngine;

public sealed class DogEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        deathClip = Resources.LoadAll<AudioClip>("Enemyes\\Death\\BloodDog\\");
    }
    protected override void LookOnTarget()
    {
        Vector3 startRot = transform.localEulerAngles;
        transform.LookAt(target);
        transform.localEulerAngles = new Vector3(startRot.x, transform.localEulerAngles.y, 0);
    }
}
