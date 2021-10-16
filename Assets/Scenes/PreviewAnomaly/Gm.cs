using Society.Patterns;

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