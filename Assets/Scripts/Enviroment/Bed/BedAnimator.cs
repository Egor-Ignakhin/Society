
using Society.GameScreens;

using UnityEngine;

namespace Society.Enviroment.Bed
{
    public class BedAnimator : MonoBehaviour
    {
        [SerializeField] private Animator mAnimator;
        private BedMesh bedMesh;
        private Transform lastPlayerParent;

        public void Deoccupied(Transform lastPlayerParent, BedMesh bedMesh)
        {               
            this.bedMesh = bedMesh;
            this.lastPlayerParent = lastPlayerParent;


            mAnimator.enabled = true;
            mAnimator.SetTrigger("WakeUp");
        }

        public void AnimDeoccipied()
        {
            ScreensManager.SetScreen(null);

            //Возвращение позиций в исходное значение
            Camera.main.transform.SetParent(lastPlayerParent);
            Camera.main.transform.localScale = Vector3.one;

            bedMesh.SetOccupied(false);// кровать больше не занята
        }
    }
}