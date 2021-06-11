using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace S.A
{
    public abstract class SteelArms : MonoBehaviour
    {
        [SerializeField] private Transform rayStartPos;
        [SerializeField] private Inventory.ItemStates.ItemsID id;

        public int GetID() => (int)id;
        public void FinishAnimation() => transform.parent.GetComponent<StellArmsAnimator>().FinishAnimation();

        public void SetPossibleDestroy()=> transform.parent.GetComponent<StellArmsAnimator>().SetPossibleDestroy(true);
        public Transform GetRSP() => rayStartPos;
    }
}