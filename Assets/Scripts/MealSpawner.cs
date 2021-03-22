using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MealSpawner : MonoBehaviour
{
    public static int CountOnScene;
    private void Awake()
    {
        StartCoroutine(nameof(SpawnMeal));
    }
    private IEnumerator SpawnMeal()
    {
        while (true)
        {
            if(CountOnScene < 5)
            {
                CountOnScene++;
                GameObject milk = EatingObjectsPool.FromPool(EatingObjectsPool.type.milk);
                milk.transform.position = new Vector3(transform.position.x + Random.Range(-5,5), transform.position.y, transform.position.z + Random.Range(-5, 5));
            }
            yield return new WaitForSeconds(5);
        }
    }
}
