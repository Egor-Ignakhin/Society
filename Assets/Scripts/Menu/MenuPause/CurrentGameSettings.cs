using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Society.Menu.MenuPause
{
    [System.Serializable]
    public class CurrentGameSettings
    {
        public float minFov = 60;
        public float FOV = 70;
        public float maxFov = 80;

        public bool BloomEnabled = true;

        public int MinSensivity = 0;
        public int Sensivity = 3;
        public int MaxSensivity = 10;

        public bool reloadEffectEnabled = true;
    }
}