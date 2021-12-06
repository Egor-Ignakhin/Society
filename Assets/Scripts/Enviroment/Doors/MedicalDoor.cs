using UnityEngine;
namespace Society.Enviroment.Doors
{
    public sealed class MedicalDoor : MonoBehaviour
    {
        private Animator mAnimator;
        private void Awake() => mAnimator = GetComponent<Animator>();
        internal void OnEnterPlayer() => mAnimator.CrossFade("Open", 1);

        internal void OnExitPlayer() => mAnimator.CrossFade("Close", 1);
    }
}