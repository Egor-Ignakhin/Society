using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Society.Player.Controllers;

using UnityEngine;

namespace Society.Missions
{
    public class ReflectionMission_0 : MonoBehaviour
    {
        private bool taskIsStarted;
        private Transform playerCameraTr;
        [SerializeField] private Transform point;
        private void Start()
        {
            playerCameraTr = FindObjectOfType<FirstPersonController>().GetCamera().transform;
        }
        internal void StartTask()
        {
            taskIsStarted = true;

        }
        private void Update()
        {
            if (!taskIsStarted)
                return;

            playerCameraTr.position = Vector3.MoveTowards(playerCameraTr.position, point.position, 1);
            playerCameraTr.eulerAngles = Vector3.MoveTowards(playerCameraTr.eulerAngles, point.eulerAngles, 1);
                 
        }
    }
}