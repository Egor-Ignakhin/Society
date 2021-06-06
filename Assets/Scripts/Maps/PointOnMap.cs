using UnityEngine;

namespace Maps
{
    public class PointOnMap : MonoBehaviour
    {
        private bool itsGameObject = true;

        [SerializeField] private float minScale = 0.85f;
        [SerializeField] private float maxScale = 1.15f;
        private float currentMultiply = 1;
        public Vector3 DefaultScale { get; private set; }

        [SerializeField] [Range(0, 5)] private float Speed = 0.25f;

        private Transform mTransformIn3d;
        private MapManager mapManager;
        private void Awake()
        {
            if (itsGameObject)
                mTransformIn3d = transform;
        }
        private void Start()
        {            
            mapManager = FindObjectOfType<MapDrawer>().MMapManager;
            AddPointToMap();
        }
        public void AddPointToMap()
        {
            if (!itsGameObject)
            {
                DefaultScale = transform.localScale;
                return;
            }
            mapManager.Add3DObject(transform);
        }
        private void FixedUpdate()
        {
            PlayAnimation();
        }
        private void PlayAnimation()
        {
            if (itsGameObject)
                return;
            if (transform.localScale == DefaultScale * currentMultiply)
                SetMultiplyAnimation();
            Vector3 nextPos = (DefaultScale * currentMultiply);
            transform.localScale = Vector3.MoveTowards(transform.localScale, nextPos, Time.fixedDeltaTime * (maxScale / minScale) * Speed);
        }
        private void SetMultiplyAnimation()
        {
            currentMultiply = currentMultiply == minScale ? maxScale : minScale;
        }
        public void SetState(bool value)
        {
            itsGameObject = value;
        }
        public Transform GetMy3dTransform()
        {
            return mTransformIn3d;
        }
        public void SetMy3dTransform(Transform t)
        {
            mTransformIn3d = t;
        }
        private void OnDestroy()
        {
            if (itsGameObject)
                return;
            mapManager.Remove3DObject(mTransformIn3d, this);
        }
    }
}