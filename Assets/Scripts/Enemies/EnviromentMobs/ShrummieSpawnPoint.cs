using UnityEngine;

namespace Society.Monsters.EnviromentMobs
{
    sealed class ShrummieSpawnPoint : MonoBehaviour
    {
        private void Awake()
        {
            FindObjectOfType<ShrummiesManager>().AddSpawnPoint(transform.position);
        }
    }
}