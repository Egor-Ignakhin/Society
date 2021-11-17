using Society.Player;
using Society.Player.Controllers;

using UnityEngine;

namespace Society.Parkour
{   /// <summary>
    /// Элемент паркура - верёвка с карабином
    /// </summary>
    internal sealed class RopeCarabiner : ParkourElement
    {
        [SerializeField] private Transform highestPlace;
        [SerializeField] private Transform lowestPlace;
        private readonly float minimumVert = -45.0f;
        private readonly float maximumVert = 45.0f;

        private void Start()
        {
            fpc = FindObjectOfType<FirstPersonController>();
            cameraTransform = fpc.GetCamera().transform;
            input = new RopeCarabinerInput();
            input.SetClimbMethod(Climb);
        }
        public override void Interact()
        {
            if (isInteracted)
                return;
            Society.GameScreens.ScreensManager.SetScreen(this, false);
            float startPosY = Mathf.Clamp(playerInteractive.GetHitPoint().y, lowestPlace.position.y, highestPlace.position.y);
            animatorParent.position = new Vector3(animatorParent.position.x, startPosY, animatorParent.position.z);

            cameraTransform.SetParent(animatorParent);
            cameraTransform.localScale = Vector3.one;
            posFpcOnStartClimbing = fpc.transform.position;
            BasicNeeds.Instance.SetEnableDamageFromCollision(false);

            isInteracted = true;
        }
        public void LateUpdate()
        {
            if (isInteracted)
            {
                LockCameraAndFpcTransform();
                RotateCameraAroundRope();
                input.CheckSystemInput();
            }
        }
        /// <summary>
        /// тело метода вращает камеру вокруг веревки при движении мышкой
        /// </summary>
        private void RotateCameraAroundRope()
        {
            float rotationX = -Input.GetAxis("Mouse Y");
            float rotationY = Input.GetAxis("Mouse X");
            Vector3 target = new Vector3(lowestPlace.position.x, animatorParent.position.y, lowestPlace.position.z);
            animatorParent.RotateAround(target, Vector3.up, rotationY);
            animatorParent.localEulerAngles += new Vector3(rotationX, 0, 0);

            Vector3 cAngles = animatorParent.eulerAngles;
            if (cAngles.x < 315 && cAngles.x > maximumVert * 2)
                animatorParent.eulerAngles = new Vector3(minimumVert, cAngles.y, cAngles.z);
            if (cAngles.x < maximumVert * 2 && cAngles.x > maximumVert)
                animatorParent.eulerAngles = new Vector3(maximumVert, cAngles.y, cAngles.z);
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
        private void Climb(bool isUp, bool isAcceleration)
        {
            float cSpeed = isAcceleration ? speedClimbing * 5 : speedClimbing;
            Vector3 nextPos = (animatorParent.position + new Vector3(0, (isUp ? 1 : -1) * Time.deltaTime * cSpeed, 0));
            nextPos.y = Mathf.Clamp(nextPos.y, lowestPlace.position.y, highestPlace.position.y);
            animatorParent.position = nextPos;
        }
        private void Jumpoff()
        {
            fpc.SetPosAndRot(cameraTransform);
            cameraTransform.SetParent(fpc.GetCameraHost());
            cameraTransform.localScale = Vector3.one;
            isInteracted = false;
            fpc.SetPossibleJump(false);
            BasicNeeds.Instance.SetEnableDamageFromCollision(true);
            fpc.ResetRbVelocity();
        }
        public override bool Hide()
        {
            Jumpoff();
            return true;
        }

        public override KeyCode HideKey() => KeyCode.Space;

        public class RopeCarabinerInput : ParkoutInput
        {
            public override void CheckSystemInput()
            {
                if (Input.GetKey(KeyCode.W))// вверх
                {
                    climbMethod(true, false);
                }
                else if (Input.GetKey(KeyCode.LeftControl))// вниз
                {
                    climbMethod(false, true);
                }
            }
        }
    }
}