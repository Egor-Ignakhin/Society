using Society.Patterns;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Society.Anomalies.Apiary
{
    public class ApiaryBulletReceiver : MonoBehaviour, IBulletReceiver
    {
        [SerializeField] private ApiaryManager apiaryManager;
        public void OnBulletEnter()
        {
            apiaryManager.Hit();
        }
    }
}