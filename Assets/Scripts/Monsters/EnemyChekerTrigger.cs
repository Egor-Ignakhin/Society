using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemies
{
    public class EnemyChekerTrigger : MonoBehaviour
    {
        [SerializeField] BredEnemy mBred;
        private void OnTriggerStay(Collider other)
        {
            mBred.CheckIntersects(other);
        }
    }
}