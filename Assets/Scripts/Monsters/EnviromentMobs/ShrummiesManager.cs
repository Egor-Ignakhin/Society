using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Society.Monsters.EnviromentMobs
{
    public sealed class ShrummiesManager : MonoBehaviour
    {
        private readonly List<Vector3> spawnPoints = new List<Vector3>();
        private readonly List<ShrummiExample> freeExamples = new List<ShrummiExample>();

        private void Start()
        {
            StartCoroutine(nameof(UpdateCycle));
        }
        internal void AddSpawnPoint(Vector3 spawnPoint) => spawnPoints.Add(spawnPoint);

        private IEnumerator UpdateCycle()
        {
            while (true)
            {
                for (int i = 0; i < freeExamples.Count; i++)
                {
                    var spawnedPosition = GenerateRandomPosition();
                    freeExamples[i].OnInit(spawnedPosition,
                        GetNearestPosition(spawnedPosition), this);
                    freeExamples.RemoveAt(i--);
                }
                yield return new WaitForSeconds(1);
            }
        }

        internal void AddFreeExample(ShrummiExample se)
        {
            freeExamples.Add(se);
        }
        private Vector3 GetNearestPosition(Vector3 currentPosition)
        {
            Vector3 returnPosition = Vector3.positiveInfinity;

            float returnDistance = float.MaxValue;
            float tempDistance;

            foreach (var point in spawnPoints)
            {
                if (point == currentPosition)
                    continue;

                tempDistance = Vector3.Distance(point, currentPosition);
                if (tempDistance < returnDistance)
                {
                    returnDistance = tempDistance;
                    returnPosition = point;
                }
            }

            return returnPosition;
        }
        private Vector3 GenerateRandomPosition()
        {            
            int index = Random.Range(0, spawnPoints.Count);            

            return spawnPoints[index];
        }
        public void HideExample(ShrummiExample example)
        {
            AddFreeExample(example);
        }
    }
}