
using System.Collections.Generic;

using UnityEngine;

namespace Society.Enemies
{
    /// <summary>
    /// наследники класса - враги, их можно убивать. 
    /// </summary>


    public sealed class TargetPointsManager
    {
        private readonly List<EnemyPoint> points = new List<EnemyPoint>();
        private int currentPointIt = 0;

        public TargetPointsManager(Transform pointsParent)
        {
            points.AddRange(pointsParent.GetComponentsInChildren<EnemyPoint>());
        }
        private void CalculateNextPoint()
        {
            if (points[currentPointIt].GetDelay() > 0)
                return;
            else
            {
                points[currentPointIt].ResetDelay();
            }
            currentPointIt++;
            if (currentPointIt >= points.Count)
                currentPointIt = 0;
        }
        public Transform GetCurrentTarget(Enemy e)
        {
            if (points.Count > 0)
            {
                var dist = e.CalculateRemainingDistance(points[currentPointIt].transform.position);
                if (dist < e.GetAttackDistance() * 1.5f)
                    CalculateNextPoint();

                return points[currentPointIt].transform;
            }
            else return e.transform;
        }
    }
}
