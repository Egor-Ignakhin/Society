using System.IO;
using UnityEngine;

namespace MenuScripts
{
    sealed class MenuClock : MonoBehaviour
    {
        [SerializeField] private Transform[] pointers = new Transform[2];
        [SerializeField] private Vector3 additionalRotateForMin;
        [SerializeField] private Vector3 additionalRotateForHours;
        private Vector3 Mouse;
        private void OnEnable()
        {
            float s = 0;
            float m = 0;
            float h = 0;
            try
            {
                string data = File.ReadAllText(Times.WorldTime.dateFolder + Times.WorldTime.dateFile);
                Times.Date currentDate = JsonUtility.FromJson<Times.Date>(data);
                s = currentDate.seconds;
                m = currentDate.minutes;
                h = currentDate.hours;
            }
            catch { }

            RenderOnPointers(s, m, h);
        }
        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 p = ray.origin + ray.direction * 10.0f;
            transform.position = p;
        }
        private void RenderOnPointers(float sec, float min, float hours)
        {
            // поворот всех стрелок относительно текущего времени
            pointers[0].localRotation = Quaternion.Euler(new Vector3(0, min / 60 * 360 + sec / 10, 0) + additionalRotateForMin);
            pointers[1].localRotation = Quaternion.Euler(new Vector3(0, hours * 30 + min / 2, 0) + additionalRotateForHours);
        }
    }
}