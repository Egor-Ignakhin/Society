
using Society.GameScreens;
using Society.Missions;
using Society.Missions.TaskSystem;
using Society.Player.Controllers;

using UnityEngine;

namespace Society.Enviroment.Bed
{
    public class BedAnimator : MonoBehaviour
    {
        [SerializeField] private Animator mAnimator;
        private BedMesh bedMesh;
        private Transform lastPlayerParent;

        private Player.PlayerInteractive playerInteractive;
        private FirstPersonController fpc;        
        private void Awake()
        {
            playerInteractive = FindObjectOfType<Player.PlayerInteractive>();
            fpc = FindObjectOfType<FirstPersonController>();
        }

        public void Deoccupied(Transform lastPlayerParent, BedMesh bedMesh)
        {
            playerInteractive.CanPlayerInteract = false;
            MissionsManager.Instance.DescriptionDrawer.SetIrremovableHint(null);
            FindObjectOfType<CommentDrawer>().Push(Localization.LocalizationManager.Translate(Localization.LanguageIdentifiers.Prologue_firstComment));
            this.bedMesh = bedMesh;
            this.lastPlayerParent = lastPlayerParent;


            mAnimator.enabled = true;
            mAnimator.SetTrigger("WakeUp");
        }

        public void AnimDeoccipied()
        {
            playerInteractive.CanPlayerInteract = true;
            fpc.transform.localPosition = new Vector3(47.32485f, -8.196042f, -13.98516f);
            MissionsManager.Instance.TaskDrawer.SetVisible(true);
            fpc.SetRotation(Quaternion.Euler(0, 90,0));
            ScreensManager.SetScreen(null);

            //Возвращение позиций в исходное значение
            Camera.main.transform.SetParent(lastPlayerParent);
            Camera.main.transform.localScale = Vector3.one;

            bedMesh.SetOccupied(false);// кровать больше не занята
        }
    }
}