using Society.Inventory;

using UnityEngine;

namespace Society.SteelArms
{
    public abstract class SteelArmsGeneric : MonoBehaviour
    {
        [SerializeField] private Transform rayStartPos;
        [SerializeField] private ItemStates.ItemsID id;

        public int GetID() => (int)id;
        public void FinishAnimation() => transform.parent.GetComponent<StellArmsAnimator>().FinishAnimation();

        public void SetPossibleDestroy() => transform.parent.GetComponent<StellArmsAnimator>().SetPossibleDestroy(true);
        public Transform GetRSP() => rayStartPos;
    }
}