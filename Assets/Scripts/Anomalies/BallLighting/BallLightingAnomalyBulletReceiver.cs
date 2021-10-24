using Society.Patterns;
using Society.Shoot;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Society.Anomalies.BallLighting
{
	public class BallLightingAnomalyBulletReceiver : MonoBehaviour,IBulletReceiver
	{
        [SerializeField] BallLightingAnomalyManager manager;
        public void OnBulletEnter(BulletType inputBulletType)
        {            
            manager.Hit(1);
        }

        
    }
}