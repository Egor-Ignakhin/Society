using UnityEngine;

namespace Features
{
    /// <summary>
    /// рычаг подачи воды для душа
    /// </summary>
    sealed class ShowerSwitch : InteractiveObject
    {
        [SerializeField] private ShowerExample showerExp;
        private void Start() => SetType("ClosedShowerLever");
        public override void Interact() => SetType(showerExp.ChangeIsWaterOpen() ? "OpenedShowerLever" : "ClosedShowerLever");
    }
}