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
        protected override void LookOnTarget()
        {
            Vector3 startRot = transform.localEulerAngles;
            transform.LookAt(enemy ? enemy.transform : target);
            transform.localEulerAngles = new Vector3(startRot.x, transform.localEulerAngles.y, 0);            
        }        
    }
}