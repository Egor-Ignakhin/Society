using UnityEngine;

namespace Enemies
{
    sealed class BredEnemy : Enemy
    {         
        protected override void Start()
        {
            base.Start();
            deathClip = Resources.LoadAll<AudioClip>("Enemyes\\Death\\Bred\\");
        }   
    }
}