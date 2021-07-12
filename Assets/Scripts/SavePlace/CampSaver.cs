using UnityEngine;

namespace BarrelCampScripts
{
    sealed class CampSaver : InteractiveObject// класс отвечающий за сохранение в лагере
    {
        public override void Interact() => Debug.Log("Save");
    }
}