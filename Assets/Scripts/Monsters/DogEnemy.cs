using UnityEngine;

public sealed class DogEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        deathClip = Resources.LoadAll<AudioClip>("Enemyes\\Death\\BloodDog\\");
    }
}
