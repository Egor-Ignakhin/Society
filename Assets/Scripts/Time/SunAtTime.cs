using Times;
using UnityEngine;

namespace Weather
{
    /// <summary>
    /// поворачивает солнце в зависимости от времени
    /// </summary>
    sealed class SunAtTime : MonoBehaviour
    {
        private void Start()
        {
            WorldTime.Instance.ChangeTimeEventInNumbers += Rotate;
        }

        private void Rotate(int sec, int min, int hours)
        {
            transform.localRotation = Quaternion.Euler(hours * 15 - 120 + min / 4, 180, 0);
        }
        private void OnDisable()
        {
            if (WorldTime.Instance != null)
                WorldTime.Instance.ChangeTimeEventInNumbers -= Rotate;
        }
    }
}