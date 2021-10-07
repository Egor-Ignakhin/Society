using Society.GameScreens;
using Society.Patterns;
using Society.Player.Controllers;

using System;

using UnityEngine;

namespace Society.Parkour
{
    /// <summary>
    /// базовый класс для всех элементов паркура
    /// </summary>
    public abstract class ParkourElement : InteractiveObject, IGameScreen
    {
        [SerializeField] protected float speedClimbing = 1;
        [SerializeField] protected Transform animatorParent;
        protected Transform cameraTransform;
        protected FirstPersonController fpc;
        protected Vector3 posFpcOnStartClimbing;
        protected bool isInteracted;
        protected ParkoutInput input;
        public abstract bool Hide();

        public abstract KeyCode HideKey();
        /// <summary>
        /// класс-обработчик каждого элемента паркура
        /// </summary>
        public abstract class ParkoutInput
        {
            protected Action<bool, bool> climbMethod;
            public void SetClimbMethod(Action<bool, bool> mth) => climbMethod = mth;
            public abstract void CheckSystemInput();
        }
    }
}