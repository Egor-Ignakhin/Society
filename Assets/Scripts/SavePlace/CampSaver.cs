using PlayerClasses;
using UnityEngine;

namespace BarrelCampScripts
{
    public class CampSaver : InteractiveObject// класс отвечающий за сохранение в лагере
    {
        public override void Interact(PlayerStatements pl)
        {
            Debug.Log("Save");
        }
    }
}