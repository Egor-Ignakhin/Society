using Society.Patterns;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Society.Scenes.PreviewAnomaly
{
	public class Gm : MonoBehaviour
	{
		[SerializeField] private GameObject bulletReceiver;

		public void DeathAnomaly()
        {
			bulletReceiver.GetComponent<IBulletReceiver>().OnBulletEnter();
        }
	}
}