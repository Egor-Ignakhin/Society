
using System;

using Society.Patterns;

using UnityEngine;

namespace Society.Missions
{
    /// <summary>
    /// ����� ������. ������������ �� �����.
    /// </summary>
    public sealed class TaskLabel : MonoBehaviour
    {
        private void Start()
        {
            Deactivate();
        }
        /// <summary>
        /// ��������� �����
        /// </summary>
        internal void Activate()
        {
            gameObject.SetActive(true);
        }
        private void Update()
        {
            transform.localEulerAngles += Vector3.up * Time.deltaTime * 100;
        }

        internal void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}