using System.Collections.Generic;
using UnityEngine;

namespace Maps
{
    public class MapManager
    {
        private static MapContainer mMapContainer;
        public delegate void MapHandler();
        public static event MapHandler AddPointEvent;
        public MapManager()
        {
            mMapContainer = null;
            mMapContainer = new MapContainer();
        }
        public static void Add3DObject(Transform tr)
        {
            mMapContainer.GameObjects3D.Add(tr);
            AddPointEvent?.Invoke();
        }
        public static void AddInsedPoint(PointOnMap rt)
        {
            mMapContainer.InsedPoints.Add(rt);
        }
        public List<PointOnMap> GetPoints()
        {
            return mMapContainer.InsedPoints;
        }
        public List<Transform> GetObjects()
        {
            return mMapContainer.GameObjects3D;
        }

        public static void Remove3DObject(Transform tr, PointOnMap pointOnMap)
        {
            mMapContainer.InsedPoints.Remove(pointOnMap);
            mMapContainer.GameObjects3D.Remove(tr);
        }
        public static GameObject GetTracker(GameObject object3d)
        {
            for (int i = 0; i < mMapContainer.InsedPoints.Count; i++)
            {
                if (mMapContainer.InsedPoints[i].GetMy3dTransform() == object3d.transform)
                {
                    return mMapContainer.InsedPoints[i].gameObject;
                }
            }
            return null;
        }
        public class MapContainer
        {
            public List<Transform> GameObjects3D = new List<Transform>();
            public List<PointOnMap> InsedPoints = new List<PointOnMap>();
        }

        internal Transform GetLastPoint()
        {
            return mMapContainer.GameObjects3D[mMapContainer.GameObjects3D.Count - 1];
        }
    }
}