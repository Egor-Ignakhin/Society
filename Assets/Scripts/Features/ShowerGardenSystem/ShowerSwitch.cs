using UnityEngine;

namespace Society.Features.ShowerGardenSystem
{
    /// <summary>
    /// рычаг подачи воды для душа
    /// </summary>
    internal sealed class ShowerSwitch : Society.Patterns.InteractiveObject
    {
        [SerializeField] private ShowerExample showerExp;
        private void Start() => SetType("ClosedShowerLever");
        public override void Interact() => SetType(showerExp.ChangeIsWaterOpen() ? "OpenedShowerLever" : "ClosedShowerLever");
    }
}