using System;
using UnityEngine;

namespace Parkour
{
    sealed class FireEscape : MonoBehaviour, IGameScreen
    {
        [SerializeField] private Transform highestPlace;
        [SerializeField] private Transform lowestPlace;
        private Transform cameraTransform;
        [SerializeField] private Transform animatorParent;
        private bool isInteract;
        private FireEscapeInput input;
        [SerializeField] private float speedClimbing;

        private float playerHeight;
        private FirstPersonController fpc;
        private Vector3 posFpcOnStartClimbing;
        private void Awake()
        {
            fpc = FindObjectOfType<FirstPersonController>();
            cameraTransform = fpc.GetCamera().transform;
            playerHeight = fpc.GetPlayerHeight();
            input = new FireEscapeInput(Climb);
        }
        /// <summary>
        /// Начинает анимацию и перемещает аниматор в стартовую позицию
        /// </summary>        
        public void Interact(float stepPosY)
        {
            if (isInteract)
                return;
            ScreensManager.SetScreen(this, false);
            stepPosY = Mathf.Clamp(stepPosY, lowestPlace.position.y, highestPlace.position.y + playerHeight);
            animatorParent.position = new Vector3(animatorParent.position.x, stepPosY + playerHeight, animatorParent.position.z);

            cameraTransform.SetParent(animatorParent);
            cameraTransform.localScale = Vector3.one;
            posFpcOnStartClimbing = fpc.transform.position;
            PlayerClasses.BasicNeeds.Instance.SetPossibleDamgeFromCollision(false);
            isInteract = true;
        }

        public void LateUpdate()
        {
            if (isInteract)
            {
                LockCameraAndFpcTransform();
                input.CheckSystemInput();
            }
        }
        /// <summary>
        /// блокировка позиций и ротаций камеры и капсулы
        /// </summary>
        private void LockCameraAndFpcTransform()
        {
            cameraTransform.localEulerAngles = Vector3.zero;
            cameraTransform.localPosition = Vector3.zero;
            fpc.transform.position = posFpcOnStartClimbing;
        }
        /// <summary>
        /// метод "Лезть"
        /// </summary>
        /// <param name="isUp"></param>
        /// <param name="isAcceleration"></param>
        private void Climb(bool isUp, bool isAcceleration)
        {
            float cSpeed = isAcceleration ? speedClimbing * 2 : speedClimbing;
            Vector3 nextPos = (animatorParent.position + new Vector3(0, (isUp ? 1 : -1) * Time.deltaTime * cSpeed, 0));
            nextPos.y = Mathf.Clamp(nextPos.y, lowestPlace.position.y + playerHeight, highestPlace.position.y + playerHeight);

            animatorParent.position = nextPos;
        }
        /// <summary>
        /// Метод спрыгивания
        /// </summary>
        private void JumpOff()
        {
            fpc.SetPosAndRot(cameraTransform);
            cameraTransform.SetParent(fpc.GetCameraHost());
            cameraTransform.localScale = Vector3.one;
            isInteract = false;
            ScreensManager.SetScreen(null);
            fpc.SetPossibleJump(false);
            PlayerClasses.BasicNeeds.Instance.SetPossibleDamgeFromCollision(true);
            fpc.ResetRbVelocity();
        }

        public bool Hide()
        {
            JumpOff();
            return true;
        }

        public KeyCode HideKey() => KeyCode.Space;

        /// <summary>
        /// Обработчик ввода игрока в режиме лазанья
        /// </summary>
        public sealed class FireEscapeInput
        {
            private readonly Action<bool, bool> climbMethod;// метод "лезть"
            public FireEscapeInput(Action<bool, bool> climbMethod)
            {
                this.climbMethod = climbMethod;
            }
            public void CheckSystemInput()
            {
                bool isAcceleration = Input.GetKey(KeyCode.LeftShift);// ускорение

                if (Input.GetKey(KeyCode.W))// вверх
                {
                    climbMethod(true, isAcceleration);
                }
                else if (Input.GetKey(KeyCode.S))// вниз
                {
                    climbMethod(false, isAcceleration);
                }
            }
        }
    }
}