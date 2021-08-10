using UnityEngine;

namespace Parkour
{
    /// <summary>
    /// Элемент паркура - пожарная лестница
    /// </summary>
    sealed class FireEscape : ParkourElement
    {
        private Vector3 highestPlace;
        private Vector3 lowestPlace;        

        private float playerHeight;
        private void Start()
        {
            highestPlace = transform.position;
            lowestPlace = transform.position;
            highestPlace.y = GetComponent<BoxCollider>().bounds.max.y;
            lowestPlace.y = GetComponent<BoxCollider>().bounds.min.y;
            fpc = FindObjectOfType<FirstPersonController>();
            cameraTransform = fpc.GetCamera().transform;
            playerHeight = fpc.GetPlayerHeight();
            input = new FireEscapeInput();
            input.SetClimbMethod(Climb);
        }
        /// <summary>
        /// Начинает анимацию и перемещает аниматор в стартовую позицию
        /// </summary>        
        public override void Interact()
        {
            if (isInteracted)
                return;
            ScreensManager.SetScreen(this, false);
            float stepPosY = playerInteractive.GetHitPoint().y;
            stepPosY = Mathf.Clamp(stepPosY, lowestPlace.y, highestPlace.y + playerHeight);
            animatorParent.position = new Vector3(animatorParent.position.x, stepPosY + playerHeight, animatorParent.position.z);

            cameraTransform.SetParent(animatorParent);
            cameraTransform.localScale = Vector3.one;
            posFpcOnStartClimbing = fpc.transform.position;
            PlayerClasses.BasicNeeds.Instance.SetPossibleDamgeFromCollision(false);
            isInteracted = true;
        }

        public void LateUpdate()
        {
            if (isInteracted)
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
            nextPos.y = Mathf.Clamp(nextPos.y, lowestPlace.y + playerHeight, highestPlace.y + playerHeight);

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
            isInteracted = false;
            fpc.SetPossibleJump(false);
            PlayerClasses.BasicNeeds.Instance.SetPossibleDamgeFromCollision(true);
            fpc.ResetRbVelocity();
        }

        public override bool Hide()
        {
            JumpOff();
            return true;
        }

        public override KeyCode HideKey() => KeyCode.Space;

        /// <summary>
        /// Обработчик ввода игрока в режиме лазанья
        /// </summary>
        public sealed class FireEscapeInput : ParkoutInput
        {
            public override void CheckSystemInput()
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