using System.Collections;
using System.Collections.Generic;
using Times;
using UnityEngine;

namespace Weather
{
    /// <summary>
    /// поворачивает солнце в зависимости от времени
    /// </summary>
    public class SunAtTime : MonoBehaviour
    {
        [SerializeField] private Transform sunT;

        private float defaultY;
        private void OnEnable()
        {
            WorldTime.Instance.ChangeTimeEventInNumbers += Rotate;
        }

        private void Rotate(int sec, int min, int hours)
        {
            sunT.transform.localRotation = Quaternion.Euler(hours * 15 - 120, 180, 0);
        }
        private void OnDisable()
        {
            if (WorldTime.Instance != null)
                WorldTime.Instance.ChangeTimeEventInNumbers -= Rotate;
        }
    }
}