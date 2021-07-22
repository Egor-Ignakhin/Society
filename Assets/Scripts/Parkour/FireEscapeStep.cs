using UnityEngine;
namespace Parkour
{/// <summary>
/// Класс помощник для пожарной лестницы.
/// Благодаря ему можно понять, куда нажал игрок, и куда его переместит - вниз или вверх (в начале анимации)
/// </summary>
    sealed class FireEscapeStep : InteractiveObject
    {
        [SerializeField] private FireEscape mFireEscape;        
        public override void Interact()
        {
            mFireEscape.Interact(playerInteractive.GetHitPoint().y);
        }
    }
}