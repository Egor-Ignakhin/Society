using UnityEngine;

namespace BarrelCampScripts
{
    public sealed class BarrelCampManager : MonoBehaviour// класс отвечающий за горящую бочку
    {
        private bool playerIsInside;
        public void InsidePlayer()
        {
            playerIsInside = true;
        }
        public void OutsidePlayer()
        {
            playerIsInside = false;
        }
    }
}