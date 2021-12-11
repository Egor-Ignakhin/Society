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
            //Можно вырубить свет?
            bool canTurnOffLight = true;

            //Проход по всем триггерам
            foreach (var lzt in LightZoneTriggers)
            {
                //Если в триггере находится игрок
                if (lzt.Value == true)
                {
                    //Свет вырубать нельзя
                    canTurnOffLight = false;
                    break;
                }
            }

            foreach (var l in lights)
                l.enabled = !canTurnOffLight;
        }
    }
}