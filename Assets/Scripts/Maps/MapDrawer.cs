using UnityEngine;

namespace Maps
{
    public class MapDrawer : MonoBehaviour
    {
        [SerializeField] private RectTransform mapRect;

        private Vector3 currentPos;

        private Vector3 currentRot = new Vector3(0, 0, 0);

        public MapManager MMapManager;
        public delegate void RotateHandler(Vector3 rotate);
        public event RotateHandler RotateEvent;

        [SerializeField] private GameObject PointForInstance;
        private Transform playerT;
        private void OnEnable()
        {
            MMapManager = new MapManager();
            MapManager.AddPointEvent += this.AddPoint;
            playerT = FindObjectOfType<FirstPersonController>().transform;
        }    
        private void FixedUpdate() => SetCurrentRect();

        public void SetCurrentRect()
        {
            currentPos = playerT.position;
            currentRot.z = playerT.localEulerAngles.y - 180;
            DrawMap();
        }
        private void DrawMap()
        {
            mapRect.localPosition = currentPos;
            transform.localRotation = Quaternion.Euler(currentRot);
            MMapManager.SetPositionsPoint();          
            RotateEvent?.Invoke(currentRot);
        }
        private void AddPoint()
        {
            Transform point = MMapManager.GetLastPoint();
            CreatePoint(point);
        }
        private void CreatePoint(Transform point)
        {
            PointOnMap sourcePm = point.GetComponent<PointOnMap>();
            PointOnMap receiverPm = Instantiate(PointForInstance, mapRect).AddComponent<PointOnMap>();



            foreach (var field in typeof(PointOnMap).GetFields())
            {
                field.SetValue(receiverPm, field.GetValue(sourcePm));
            }
            receiverPm.SetMy3dTransform(sourcePm.GetMy3dTransform());
            Destroy(sourcePm);
            receiverPm.SetState(false);
            MMapManager.AddInsedPoint(receiverPm);
        }

        private void OnDisable()
        {
            MMapManager = null;
            MapManager.AddPointEvent -= this.AddPoint;
        }
    }
}