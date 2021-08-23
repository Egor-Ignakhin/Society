using UnityEngine;
namespace Features
{
    /// <summary>
    /// Класс душа. Душом можно управлять: двигать по 2 осям и поливать пол.
    /// </summary>
    public sealed class ShowerManager : MonoBehaviour
    {        
        private ShowerExample shower;
        private PlayerClasses.PlayerInteractive playerInteractive;


        [SerializeField] private Transform xzCenter;
        [SerializeField] private float speed = 5;

        [SerializeField] private Transform xRope;// веревка по оси x
        [SerializeField] private Transform zRope;// веревка по оси z

        [SerializeField] private Transform minXPoint;
        [SerializeField] private Transform maxXPoint;
        [SerializeField] private Transform minZPoint;
        [SerializeField] private Transform maxZPoint;

        [SerializeField] private Transform reloadPoint;
        [SerializeField] private ShowerLeverOnWall leverOnWall;
        private Vector3 lastXzCenterPos;


        private void Awake()
        {
            playerInteractive = FindObjectOfType<PlayerClasses.PlayerInteractive>();
        }
        internal void OnInteract(ShowerExample shower)
        {            
            this.shower = shower;
        }
        private void Update()
        {
            if (shower)
            {
                MoveShowerToPlayerPointer();
                MoveShowerToReloadLever();
                MoveRopesToShowerCenter();
                if (!playerInteractive.ObjectIsDirected(shower))
                {
                    ClearReferences();
                }
            }
        }

        private void ClearReferences()
        {            
            shower = null;
        }

        private void MoveShowerToPlayerPointer()
        {           
            Vector3 shPos = xzCenter.position;
            Vector3 target = playerInteractive.GetHitPoint();
            target.y = shPos.y;

            var dist = Vector3.Distance(shPos, target);
            // если расстояние слишком близко
            if (dist > 0.1f)
            {
                Vector3 nextPos = Vector3.LerpUnclamped(shPos, target, Time.deltaTime * speed);
                nextPos.x = Mathf.Clamp(nextPos.x, minXPoint.position.x, maxXPoint.position.x);
                nextPos.z = Mathf.Clamp(nextPos.z, minZPoint.position.z, maxZPoint.position.z);
                xzCenter.position = nextPos;

                if (Vector3.Distance(xzCenter.position, lastXzCenterPos) > 0.0075f)
                    shower.OnMove();

                lastXzCenterPos = xzCenter.position;
            }
        }
        private void MoveShowerToReloadLever()
        {
            if (shower.GetIsFull())
                return;

            Vector3 shPos = xzCenter.position;
            Vector3 target = reloadPoint.position;
            target.y = shPos.y;
            if ((leverOnWall.IsLeverOpen) &&
                (!shower.GetIsFull()) && (Vector3.Distance(shPos, target) < 0.25f))
            {
                shower.SetIsFull(false);
                var nextpos = reloadPoint.position;
                nextpos.y = xzCenter.position.y;
                xzCenter.position = nextpos;
            }
        }
        private void MoveRopesToShowerCenter()
        {
            xRope.position = new Vector3(xzCenter.position.x, xRope.position.y, xRope.position.z);
            zRope.position = new Vector3(zRope.position.x, zRope.position.y, xzCenter.position.z);
        }
    }
}