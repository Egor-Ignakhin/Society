using UnityEngine;

namespace BarrelCampScripts
{
    public sealed class BarrelCampManager : MonoBehaviour// класс отвечающий за горящую бочку
    {
        private bool playerIsInside;
        public void InsidePlayer()
        {
            playerIsInside = true;
            if (playerIsInside)
            {

            }
        }
        public void OutsidePlayer()
        {
            playerIsInside = false;
        }
    }
}