using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MouseAnton
{
    public class MouseAntonManager : Singleton<MouseAntonManager>
    {
        private FirstPersonController player;
        private Vector3 supportPosition;
        [SerializeField] private GameObject prefabSource;
        private void Awake()
        {
            player = FindObjectOfType<FirstPersonController>();
            supportPosition = player.transform.position;
            StartCoroutine(nameof(AudioPlayer));
        }
        internal void SetSupportPosition(Vector3 center, Vector3 position)
        {
            supportPosition = new Vector3(position.x, center.y, position.z);
        }
        public void SetPlayerPosition()
        {
            player.transform.position = supportPosition;
        }
        private IEnumerator AudioPlayer()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(5, 10));
                Vector3 spawnedPlace = new Vector3(0, 2, 0);
                spawnedPlace.z = Random.Range(Random.Range(player.transform.position.z - 50, player.transform.position.z - 5),
                    Random.Range(player.transform.position.z + 50, player.transform.position.z + 5));

                spawnedPlace.x = Random.Range(Random.Range(player.transform.position.x - 50, player.transform.position.x - 5),
                    Random.Range(player.transform.position.x + 50, player.transform.position.x + 5));
                Instantiate(prefabSource, spawnedPlace, Quaternion.identity);
            }
        }
    }
}