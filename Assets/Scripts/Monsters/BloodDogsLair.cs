using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDogsLair : MonoBehaviour
{
    [SerializeField] private float delayToSpawn = 10f;
    [SerializeField] private GameObject dog;
    [SerializeField] private Transform spawnPlace;
    [SerializeField] private Transform parentForEnemyes;

    private bool canInst = true;
    public void InsidePlayer()
    {
        canInst = false;
    }
    public void OutsidePlayer()
    {
        canInst = true;
    }
    private void OnEnable()
    {
        StartCoroutine(nameof(DelayedSpawner));
    }
    private IEnumerator DelayedSpawner()
    {
        while (true)
        {
            if (canInst)
            {
                Instantiate(dog, spawnPlace.position, spawnPlace.rotation, parentForEnemyes);
            }
            yield return new WaitForSeconds(delayToSpawn);
        }
    }
    private void OnDisable()
    {
        StopCoroutine(nameof(DelayedSpawner));
    }
}
