using System.Collections.Generic;

using UnityEngine;

namespace Society.LightZones
{
    internal sealed class LightZoneManager : MonoBehaviour
    {
        internal Dictionary<LightZoneTrigger, bool> LightZoneTriggers { get; } = new Dictionary<LightZoneTrigger, bool>();

        [SerializeField] private List<Light> lights = new List<Light>();
        private void FixedUpdate()
        {
            //����� �������� ����?
            bool canTurnOffLight = true;

            //������ �� ���� ���������
            foreach (var lzt in LightZoneTriggers)
            {
                //���� � �������� ��������� �����
                if (lzt.Value == true)
                {
                    //���� �������� ������
                    canTurnOffLight = false;
                    break;
                }
            }

            foreach (var l in lights)
                l.enabled = !canTurnOffLight;
        }
    }
}