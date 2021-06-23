using UnityEngine;

namespace Effects
{
    public class InvItemCollision : MonoBehaviour
    {
        private DropableSoundsManager manager;
        [SerializeField] private Transform parent;
        private bool called;
        private void Start()
        {
            manager = FindObjectOfType<DropableSoundsManager>();
        }
        private void OnCollisionStay(Collision collision)
        {
            if (called)
                return;
            manager.PlayClip(parent.position, this);
            called = true;
        }
    }
}