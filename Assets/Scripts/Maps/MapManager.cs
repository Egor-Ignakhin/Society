using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maps
{
    public class MapManager
    {
        private MapData mMapContainer;
        public delegate void MapHandler();
        public static event MapHandler AddPointEvent;
        public MapManager()
        {
            mMapContainer = new MapData();
        }
        public void Add3DObject(Transform tr)
        {
            mMapContainer.GameObjects3D.Add(tr);
            AddPointEvent?.Invoke();
        }
        public void AddInsedPoint(PointOnMap rt)
        {
            mMapContainer.InsedPoints.Add(rt);
        }

        public void Remove3DObject(Transform tr, PointOnMap pointOnMap)
        {
            mMapContainer.InsedPoints.Remove(pointOnMap);
            mMapContainer.GameObjects3D.Remove(tr);
        }
        public GameObject GetTracker(GameObject object3d)
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

        internal void SetPositionsPoint()
        {
            for (int i = 0; i < mMapContainer.InsedPoints.Count; i++)
            {
                Vector3 position = new Vector3(mMapContainer.GameObjects3D[i].position.x, mMapContainer.GameObjects3D[i].position.z, 0);
                mMapContainer.InsedPoints[i].transform.localPosition = -position * mMapContainer.InsedPoints[i].DefaultScale.x;
            }
        }

        public class MapData
        {
            public List<Transform> GameObjects3D = new List<Transform>();
            public List<PointOnMap> InsedPoints = new List<PointOnMap>();
        }

        internal Transform GetLastPoint() => mMapContainer.GameObjects3D[mMapContainer.GameObjects3D.Count - 1];
    }
}