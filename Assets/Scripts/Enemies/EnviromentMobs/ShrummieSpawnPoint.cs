using UnityEngine;

namespace Society.Monsters.EnviromentMobs
{
    internal sealed class ShrummieSpawnPoint : MonoBehaviour
    {
        private void Awake()
        {
            FindObjectOfType<ShrummiesManager>().AddSpawnPoint(transform.position);
        }
    }
}