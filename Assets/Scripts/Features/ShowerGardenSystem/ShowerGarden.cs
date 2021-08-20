using UnityEngine;
namespace Features
{
    /// <summary>
    /// Класс душа. Душом можно управлять: двигать по 2 осям и поливать пол.
    /// </summary>
    public sealed class ShowerGarden : MonoBehaviour
    {
        private bool isDragged;
        private ShowerExample activeShower;
        private PlayerClasses.PlayerInteractive playerInteractive;


        [SerializeField] private Transform pivot;
        [SerializeField] private float speed = 5;

        [SerializeField] private Transform xRope;// веревка по оси x
        [SerializeField] private Transform zRope;// веревка по оси z

        [SerializeField] private Transform minXPoint;
        [SerializeField] private Transform maxXPoint;
        [SerializeField] private Transform minZPoint;
        [SerializeField] private Transform maxZPoint;

        [SerializeField] private Transform reloadPoint;
        [SerializeField] private ShowerLeverOnWall leverOnWall;
        private Vector3 lastPivotPos;


        private void Awake()
        {
            playerInteractive = FindObjectOfType<PlayerClasses.PlayerInteractive>();
        }
        internal void OnInteract(ShowerExample ashower)
        {
            isDragged = true;
            activeShower = ashower;
        }
        private void Update()
        {
            if (isDragged)
            {
                MoveShowerToPlayerPointer();
                MoveShowerToReloadLever();
                MoveRopesToShowerCenter();
                if (!playerInteractive.ObjectIsDirected(activeShower))
                {
                    ClearReferences();
                }
            }
        }

        private void ClearReferences()
        {
            isDragged = false;
            activeShower = null;
        }

        private void MoveShowerToPlayerPointer()
        {
            if (activeShower.IsReloading)
                return;

            Vector3 shPos = pivot.position;
            Vector3 target = playerInteractive.GetHitPoint();
            target.y = shPos.y;

            var dist = Vector3.Distance(shPos, target);
            // если расстояние слишком близко
            if (dist > 0.1f)
            {
                Vector3 nextPos = Vector3.LerpUnclamped(shPos, target, Time.deltaTime * speed);
                nextPos.x = Mathf.Clamp(nextPos.x, minXPoint.position.x, maxXPoint.position.x);
                nextPos.z = Mathf.Clamp(nextPos.z, minZPoint.position.z, maxZPoint.position.z);
                pivot.position = nextPos;

                if (Vector3.Distance(pivot.position, lastPivotPos) > 0.0075f)
                    activeShower.OnMove();

                lastPivotPos = pivot.position;
            }
        }
        private void MoveShowerToReloadLever()
        {
            if (activeShower.IsReloading)
                return;

            Vector3 shPos = pivot.position;
            Vector3 target = reloadPoint.position;
            target.y = shPos.y;
            if ((leverOnWall.LeverIsOpened) &&
                (!activeShower.ContentIsFilled) && (Vector3.Distance(shPos, target) < 0.25f))
            {
                activeShower.IsReloading = true;
                var nextpos = reloadPoint.position;
                nextpos.y = pivot.position.y;
                pivot.position = nextpos;
            }
        }
        private void MoveRopesToShowerCenter()
        {
            xRope.position = new Vector3(pivot.position.x, xRope.position.y, xRope.position.z);
            zRope.position = new Vector3(zRope.position.x, zRope.position.y, pivot.position.z);
        }
    }
}