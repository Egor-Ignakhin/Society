using System.Collections;
using UnityEngine;

namespace Features
{
    /// <summary>
    /// рычаг подачи воды для душа
    /// </summary>
    sealed class ShowerLever : InteractiveObject
    {
        [SerializeField] private Transform pivot;
        [SerializeField] private ShowerExample showerExp;
        [SerializeField] private float lowerY, higherY;
        private void Start()
        {
            SetType("ClosedShowerLever");
        }
        public override void Interact()
        {
            bool nextState = !showerExp.ChangeEnableWater();

            //StopAllCoroutines();
            // StartCoroutine(LeverAnimator(nextState));
            Vector3 nextRotate = new Vector3(nextState ? higherY : lowerY, pivot.localEulerAngles.y, pivot.localEulerAngles.z);
            pivot.localEulerAngles = nextRotate;
            SetType(nextState ? "ClosedShowerLever" : "OpenedShowerLever");
        }

        private IEnumerator LeverAnimator(bool nextState)
        {
            SetType("None");
            Quaternion nextRotate = new Quaternion(Quaternion.Euler((nextState ? higherY : lowerY), 0, 0).x, 0, 0, 0);
            while (true)
            {
                pivot.localRotation = Quaternion.RotateTowards(pivot.localRotation, nextRotate, 1);
                if (pivot.localRotation == nextRotate)
                {
                    SetType(nextState ? "ClosedShowerLever" : "OpenedShowerLever");
                    break;
                }
                print(1);
                yield return null;
            }
        }
    }
}