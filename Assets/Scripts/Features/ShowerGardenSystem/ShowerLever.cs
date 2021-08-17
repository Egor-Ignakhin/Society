using UnityEngine;

namespace Features
{
    /// <summary>
    /// рычаг подачи воды для душа
    /// </summary>
    sealed class ShowerLever : InteractiveObject
    {
        [SerializeField] private ShowerExample showerExp;
        private void Start() => SetType("ClosedShowerLever");
        public override void Interact() => SetType(showerExp.ChangeEnableWater() ? "OpenedShowerLever" : "ClosedShowerLever");
    }
}