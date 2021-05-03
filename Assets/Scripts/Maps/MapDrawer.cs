using UnityEngine;

namespace Maps
{
    public class MapDrawer : MonoBehaviour
    {
        [SerializeField] private RectTransform mapRect;

        private Vector3 currentPos;

        private Vector3 currentRot = new Vector3(0, 0, 0);

        private MapManager mapManager;

        [SerializeField] private GameObject PointForInstance;
        private void OnEnable()
        {
            mapManager = new MapManager();
            MapManager.AddPointEvent += this.AddPoint;
        }
        private void DrawMap()
        {
            mapRect.localPosition = currentPos;
            transform.localRotation = Quaternion.Euler(currentRot);
            var points = mapManager.GetPoints();
            var objects = mapManager.GetObjects();
            for (int i = 0; i < points.Count; i++)
            {
                SetPositionPoint(points[i], objects[i]);
            }
        }
        public void SetCurrentRect(float newX, float newY, Vector3 quat)
        {
            currentPos = new Vector2(newX, newY);

            currentRot.z = quat.y - 180;
            DrawMap();
        }
        private void AddPoint()
        {
            Transform point = mapManager.GetLastPoint();
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
            MapManager.AddInsedPoint(receiverPm);
        }
        private void SetPositionPoint(PointOnMap insedPoint, Transform point)
        {
            Vector3 position = new Vector3(point.position.x, point.position.z, 0);
            insedPoint.transform.localPosition = -position * insedPoint.defaultScale.x;
        }
        private void OnDisable()
        {
            mapManager = null;
            MapManager.AddPointEvent -= this.AddPoint;            
        }
    }
}